/*
 * main.c
 */
#include <msp430f5338.h>

#include <stdint.h>
#include <intrinsics.h>
#include <math.h>
#include "delay.h"
#include "main_support.h"
#include "oled_12864.h"
#include "finger.h"


void main(void)
{
	unsigned char i;
	WDTCTL = WDTPW | WDTHOLD;

	configure_system();

	ssd1306_init();

	ssd1306_draw_char(1,28,'H',MEDIUM_FONT);
	ssd1306_draw_char(1,42,'E',MEDIUM_FONT);
	ssd1306_draw_char(1,56,'L',MEDIUM_FONT);
	ssd1306_draw_char(1,70,'L',MEDIUM_FONT);
	ssd1306_draw_char(1,84,'O',MEDIUM_FONT);
	DELAY_US(200000);
	ssd1306_draw_char(1,28,' ',MEDIUM_FONT);
	ssd1306_draw_char(1,42,' ',MEDIUM_FONT);
	ssd1306_draw_char(1,56,' ',MEDIUM_FONT);
	ssd1306_draw_char(1,70,' ',MEDIUM_FONT);
	ssd1306_draw_char(1,84,' ',MEDIUM_FONT);

	__bis_SR_register(GIE);
	//Enable general interrupt

	//MAIN PROGRAM

	while(1)
	{
		__bis_SR_register(LPM0_bits);

		/* Heart Rate Computation */
		heart_rate = ((60*512) / (long) log_heart_signal_sample_counter)*3;
		if (heart_rate >= 255) heart_rate = 255;

		for (i=9; i; i--) heart_rate_buf[i] = heart_rate_buf[i-1];

		heart_rate_buf[0] = heart_rate;

		heart_rate_mean = 0;
		for (i = 0; i < 10; i++) heart_rate_mean = heart_rate_mean + heart_rate_buf[i];

		heart_rate_mean = heart_rate_mean / 10;

		/* SaO2 Computation */
		x = log_sq_ir_ac_level / (long) log_heart_signal_sample_counter;
		y = log_sq_red_ac_level / (long) log_heart_signal_sample_counter;
		ratio = (unsigned int) (100.0 * log(y) / log(x));

		if( ratio > 112) ratio = 112;

		SaO2 = Lookup[ratio-70];

		//Heart rate display number on OLED (a,b,c)
		a = heart_rate_mean / 100;
		b = (heart_rate_mean % 100)/10;
		c = heart_rate_mean % 10;
		if (a != 0)
		{
			a = a + '0';
			b = b + '0';
		}
		else
		{
			a = ' ';
			if(b != 0) b = b + '0';
			else b = ' ';
		}
		c = c + '0';


		//SpO2 display number on OLED (d,e,f)
		d = (SaO2 / 100) + '0';
		e = (SaO2 % 100)/10 + '0';
		f = (SaO2 % 10) + '0';

		if (finger_in)
		{
			//Remove number of heart beat display if equal 0 (first two numbers)
			ssd1306_draw_char(1,0, ' ', MEDIUM_FONT);
			ssd1306_draw_char(1,14, ' ', MEDIUM_FONT);
			ssd1306_draw_char(1,28, ' ', MEDIUM_FONT);

			//Remove residue character after finger in
			ssd1306_draw_char(1,42, ' ', MEDIUM_FONT);
			ssd1306_draw_char(1,56, ' ', MEDIUM_FONT);

			//Heart beat display
			ssd1306_draw_char(1,0, a, MEDIUM_FONT);
			ssd1306_draw_char(1,14, b, MEDIUM_FONT);
			ssd1306_draw_char(1,28, c, MEDIUM_FONT);

			//SpO2 display
			ssd1306_draw_char(1,70, d, MEDIUM_FONT);
			ssd1306_draw_char(1,84, e, MEDIUM_FONT);
			ssd1306_draw_char(1,98, f, MEDIUM_FONT);
			ssd1306_draw_char(1,112, '%', MEDIUM_FONT);
		}
	}
}

#pragma vector = TIMER0_A0_VECTOR
__interrupt void TIMER0_A0_ISR(void)
{
	if (DAC12_0CTL0 & DAC12OPS)					//If previously is P2.2 - P7.6 (IR LED)
	{
		DAC12_0CTL0 &= ~DAC12ENC;
		P2OUT &= ~BIT3;
		DAC12_0CTL0 &= ~DAC12OPS;
		DAC12_0CTL0 |= DAC12ENC;
		DAC12_0DAT = red_led_intensity;
		DAC12_1DAT = red_dc_offset;
		P2OUT |= BIT2;
		is_IR = 0;
		//Load result from previous ADC
		ir_raw = ADC12MEM0;						//Read from OPA381
		ir_amplified = ADC12MEM1;				//Read from OPA333

		ADC12CTL0 &= ~ ADC12ENC;
		ADC12CTL0 |= ADC12ENC;					//Procedure to restart ADC conversion sequence

		//Implement low pass FIR filter and IIR DC extraction filter
		ir_lowpass_filtered = ir_filter(ir_amplified);
//		ir_dc_level = dc_extraction(&ir_dc_register, ir_lowpass_filtered);
//		ir_ac_level_2 = ir_dc_level - dc_extraction(&ir_dc_register2, ir_dc_level);

		ir_ac_level = ir_lowpass_filtered - dc_extraction(&ir_dc_register, ir_lowpass_filtered);

		sq_ir_ac_level += ( ((long) ir_ac_level * (long) ir_ac_level) >> 10 );

		ir_ac_level += 2048;
		if(buf_ir_count < 2000)
		{
			buf_scaled_ir[buf_ir_count] = ir_ac_level;
			buf_ir_count++;
		}
		else buf_ir_count = 0;

		//Process data
/*		if (auto_offset)
		{
			if (ir_amplified >= AMPLIFIED_SIGNAL_HIGH)
			{
				if (ir_dc_offset > DC_OFFSET_MIN)
					ir_dc_offset--;
			}
			else if (ir_amplified < AMPLIFIED_SIGNAL_LOW)
			{
				if (ir_dc_offset < DC_OFFSET_MAX)
					ir_dc_offset++;
			}
		}*/

		//Auto Intensity
		if (ir_raw >= RAW_SIGNAL_HIGH || ir_raw <= RAW_SIGNAL_LOW)
		{
			if (ir_raw >= RAW_SIGNAL_HIGH)
			{
				if ( ir_raw >= RAW_SIGNAL_VERY_HIGH )
					ir_led_intensity -= FAST_CORRECTION;
				else
					ir_led_intensity -= SLOW_CORRECTION;

				if (ir_led_intensity < INTENSITY_MIN )
					ir_led_intensity = INTENSITY_MIN;
			}
			else if (ir_raw <= RAW_SIGNAL_LOW)
			{
				if ( ir_raw <= RAW_SIGNAL_VERY_LOW )
					ir_led_intensity += FAST_CORRECTION;
				else
					ir_led_intensity += SLOW_CORRECTION;

				if (ir_led_intensity > INTENSITY_MAX )
					ir_led_intensity = INTENSITY_MAX;
			}
		}

		//Detect finger off the sensor
		if (ir_led_intensity < 1100) finger_in = 0;
		else finger_in = 1;

		if ((finger_in == 0) && (finger_in != finger_previous_state))
		{
			ssd1306_draw_image_full((char *) finger);
			ssd1306_draw_char(1,0,'N',MEDIUM_FONT);
			ssd1306_draw_char(1,14,'o',MEDIUM_FONT);
			ssd1306_draw_char(1,28,' ',MEDIUM_FONT);
			ssd1306_draw_char(1,42,'F',MEDIUM_FONT);
			ssd1306_draw_char(1,56,'i',MEDIUM_FONT);
			ssd1306_draw_char(1,70,'n',MEDIUM_FONT);
			ssd1306_draw_char(1,84,'g',MEDIUM_FONT);
			ssd1306_draw_char(1,98,'e',MEDIUM_FONT);
			ssd1306_draw_char(1,112,'r',MEDIUM_FONT);
		}

		if ((finger_in == 1) && (finger_in != finger_previous_state)) ssd1306_clear_all();

		finger_previous_state = finger_in;

		//Drawing plethysmograph on OLED

		draw_skip_count++;
		if (finger_in && draw_skip_count > 10)
		{
			scaled_ir_ac_level_2 = (buf_scaled_ir[buf_ir_count] / 80) - 5;					//First, scale the value from ir_ac_level2 to fit the screen (0 - 39)
			ssd1306_draw_plethysmograph(scaled_ir_ac_level_2);
			draw_skip_count = 0;
		}

		//Heart beat detection
		heart_signal_sample_counter++;

		if(rising_edge)
		{
			if(prev_ir_ac_level <= ir_ac_level)
			{
				debounce_counter = 0;
				prev_ir_ac_level = ir_ac_level;
			}
			else
			{
				debounce_counter++;
				if ( (debounce_counter >= THRESHOLD)
					&& (prev_ir_ac_level > ir_ac_level) )			// the edge changed
				{
					rising_edge = 0;
					debounce_counter = 0;
					heart_beat_halfcycle_count++;
				}
			}
		}
		else														//if current edge is falling edge
		{
			if(prev_ir_ac_level >= ir_ac_level)
			{
				debounce_counter = 0;
				prev_ir_ac_level = ir_ac_level;
			}
			else
			{
				debounce_counter++;
				if( (debounce_counter >= THRESHOLD) && (prev_ir_ac_level < ir_ac_level) )
				{
					rising_edge = 1;
					debounce_counter = 0;
					heart_beat_halfcycle_count++;
				}
			}
		}

		if (heart_beat_halfcycle_count >= NO_OF_HALFCYCLES)
		{
			log_heart_signal_sample_counter = heart_signal_sample_counter;
			log_sq_ir_ac_level = sq_ir_ac_level;
			log_sq_red_ac_level = sq_red_ac_level;
			sq_ir_ac_level = 0;
			sq_red_ac_level = 0;
			heart_beat_halfcycle_count = 0;
			debounce_counter = 0;
			heart_signal_sample_counter = 0;
			__bic_SR_register_on_exit(LPM0_bits);
		}

	}
	else										//If previously is P2.3 - P6.6 (RED LED)
	{
		DAC12_0CTL0 &= ~DAC12ENC;
		P2OUT &= ~BIT2;
		DAC12_0CTL0 |= DAC12OPS;
		DAC12_0CTL0 |= DAC12ENC;
		DAC12_0DAT = ir_led_intensity;
		DAC12_1DAT = ir_dc_offset;
		P2OUT |= BIT3;
		is_IR = 1;

		//Load result from previous ADC
		red_raw = ADC12MEM0;						//Read from OPA381
		red_amplified = ADC12MEM1;					//Read from OPA333

		ADC12CTL0 &= ~ADC12ENC;
		ADC12CTL0 |= ADC12ENC;						//Procedure to restart ADC conversion sequence

		red_lowpass_filtered = red_filter(red_amplified);
		red_ac_level = red_lowpass_filtered - dc_extraction(&red_dc_register, red_lowpass_filtered);
		sq_red_ac_level += ( ((long) red_ac_level * (long) red_ac_level) >> 10 );

		//Process data
/*		if (auto_offset)
		{
			if (red_amplified >= AMPLIFIED_SIGNAL_HIGH)
			{
				if (red_dc_offset > DC_OFFSET_MIN)
					red_dc_offset--;
			}
			else if (red_amplified < AMPLIFIED_SIGNAL_LOW)
			{
				if (red_dc_offset < DC_OFFSET_MAX)
					red_dc_offset++;
			}
		}*/

		if (red_raw >= RAW_SIGNAL_HIGH || red_raw <= RAW_SIGNAL_LOW)
		{
			if (red_raw >= RAW_SIGNAL_HIGH)
			{
				if ( red_raw >= RAW_SIGNAL_VERY_HIGH )
					red_led_intensity -= FAST_CORRECTION;
				else
					red_led_intensity -= SLOW_CORRECTION;

				if (red_led_intensity <= INTENSITY_MIN)
					red_led_intensity = INTENSITY_MIN;
			}
			else if (red_raw <= RAW_SIGNAL_LOW)
			{
				if ( red_raw <= RAW_SIGNAL_VERY_LOW )
					red_led_intensity += FAST_CORRECTION;
				else
					red_led_intensity += SLOW_CORRECTION;

				if (red_led_intensity > INTENSITY_MAX)
					red_led_intensity = INTENSITY_MAX;
			}
		}
	}
}

#pragma vector = ADC12_VECTOR
__interrupt void ADC12_ISR(void)
{
	ADC12IFG &= ~ADC12IFG1;

	//Reduce current consumption of DAC and H-bridge control
	DAC12_0DAT = 0;
	DAC12_1DAT = 0;
	P2OUT |= (BIT2 | BIT3);
}

#pragma vector = UNMI_VECTOR
__interrupt void UNMI_ISR(void)
{
	switch (SYSUNIV)
	{
	case 0x00: break;		//No interrupt
	case 0x02: break;		//NMIIFG
	case 0x04:
		do
		{
			UCSCTL7 &= ~(XT2OFFG | XT1LFOFFG | DCOFFG);		// Clear XT2,XT1,DCO fault flags
			SFRIFG1 &= ~OFIFG;                      		// Clear fault flags
			__delay_cycles(8192);
		}
		while (SFRIFG1 & OFIFG);                			// Test oscillator fault flag
		break;
	case 0x06: break;		//ACCVIFG
	case 0x08: break;		//BUSIFG
	}
}

int ir_filter(int sample)
{
	static long buf[32];
	static unsigned int offset = 0;
	long z;
	char i;

	//Filter hard above a few HZ using a symmetric FIR. This has benign phase characteristics
	buf[offset] = sample;
	z = coeffs[11] * buf[(offset - 11) & 0x1F];
	for (i = 0;  i < 11;  i++)
	{
		z += coeffs[i] * (buf[(offset - i) & 0x1F] + buf[(offset - 22 + i) & 0x1F]);
	}
	offset = (offset + 1) & 0x1F;
	return  z >> 15;
}

int red_filter(int sample)
{
    static long buf[32];
    static unsigned int offset = 0;
    long z;
    char i;

	//Filter hard above a few HZ using a symmetric FIR. This has benign phase characteristics
    buf[offset] = sample;
    z = coeffs[11] * buf[(offset - 11) & 0x1F];
    for (i = 0;  i < 11;  i++)
    {
        z += coeffs[i] * (buf[(offset - i) & 0x1F] + buf[(offset - 22 + i) & 0x1F]);
    }
    offset = (offset + 1) & 0x1F;
    return  z >> 15;
}

int dc_extraction(long *p, int x)
{
    /* Noise shaped DC extraction. */
    *p += ((((long) x << 16) - *p) >> 9);
    return (*p >> 16);
}

void configure_system(void)
{
	//CONFIGURE SYSTEM CLOCK
	BAKCTL |= BAKDIS;
	while(BAKCTL & LOCKBAK)					//Turn off LOCKBAK to release XT1 from RTC_B
		BAKCTL &= ~LOCKBAK;
	UCSCTL6 &= ~XT1OFF;						//Enable XT1 oscillator

	UCSCTL1 = DCORSEL_4;					//Choose the frequency range for DCO
	UCSCTL2 = 183;							//MCLK use DCOCLKDIV => MCLK = 32768 * 183 = 6,03 MHz. Oscilloscope: 6,03Mhz
	__delay_cycles(500000);					//It takes about 500ms to start up from 32768 at full strength
	do
	{
		UCSCTL7 &= ~(XT2OFFG | XT1LFOFFG | DCOFFG);
		SFRIFG1 &= ~ OFIFG;
		__delay_cycles(8192);				//If OFIFG is good after 8192 cycles, it will get cleared completely
	}
	while (SFRIFG1 & OFIFG);
	UCSCTL6 &= ~(XT1DRIVE_3);				//Reduce XT1 drive strength, default capacitance 12pF
	SFRIE1 |= OFIE;							//Enable oscillator fault interrupt on NMI to clear fault flag if it occurred

	P1DIR |= BIT0;		P3DIR |= BIT4;		//Output ACLK and SMCLK on P1.0 and P3.4
	P1SEL |= BIT0;		P3SEL |= BIT4;

	//CONFIGURE SYSTEM REFERENCE VOLTAGE
	ADC12CTL0 &= ~(ADC12ENC);				//Ensure ADC12ENC = 0, if not REFGENBUSY = 1
	DAC12_0CTL0 &= ~DAC12AMP_7;				//Ensure DAC12AMP_7 = 0, if not REFGENBUSY = 1
	DAC12_1CTL0 &= ~DAC12AMP_7;				//Ensure DAC12AMP_7 = 0, if not REFGENBUSY = 1
	while(REFCTL0 & REFGENBUSY);
	REFCTL0 |= REFMSTR | REFVSEL_3 | REFTCOFF | REFON; //VREF = 2.5V

	//CONFIGURE DAC12
	DAC12_1CTL0 &= ~DAC12ENC;				//Ensure to disable DAC12 module
	DAC12_0CTL0 &= ~DAC12ENC;

	DAC12_1CTL0 |= DAC12AMP_7;				//Configure amplifier buffer before calibration
	DAC12_0CTL0 |= DAC12AMP_7;				//Configure amplifier buffer before calibration

	DAC12_1CALCTL = DAC12PW;				//Write calibration unlock password
	DAC12_0CALCTL = DAC12PW;				//Write calibration unlock password
	DAC12_1CTL0 |= DAC12CALON;				//Start auto-calibration on DAC1
	DAC12_0CTL0 |= DAC12CALON;				//Start auto-calibration on DAC0

	do
	{
		__delay_cycles(100000);
	}
	while(DAC12_0CTL0 & DAC12CALON);		//It takes around 17ms for DAC12 to calibrate

	DAC12_1CALCTL = DAC12PW | DAC12LOCK;	//Lock calibration data
	DAC12_0CALCTL = DAC12PW | DAC12LOCK;	//Lock calibration data

	DAC12_1DAT = 0;							//Initial value for DC_offset
	DAC12_1CTL0 |= DAC12ENC;

	P2DIR |= (BIT2 | BIT3);					//P2.2 -- OPS
	P2OUT |= BIT3;
	P2OUT &= ~BIT2;
	DAC12_0DAT = red_led_intensity;			//Initial value for Probe LEDS
	DAC12_0CTL0 |= (DAC12OPS | DAC12ENC);

	//CONFIGURE ADC12

	P6SEL |= (BIT4 | BIT3);								//Disable digital part, enable analog input

	ADC12CTL0 &= ~(ADC12ENC);
	ADC12CTL0 = ADC12SHT0_4 | ADC12MSC | ADC12ON;		//64 ADC12CLK cycles, convert to EOS, enable ADC12 module
	ADC12CTL1 = ADC12SHS_1 | ADC12SHP | ADC12SSEL_3 | ADC12CONSEQ_1;	//Conversion trigger by Timer0-CCR1, internal adc12 clock, sequence of channels

	ADC12MCTL0 = ADC12SREF_1 | ADC12INCH_4;						//V(R+) = VREF+; V(R-) = VSS, channel 4
	ADC12MCTL1 = ADC12SREF_1 | ADC12INCH_5 | ADC12EOS;			//V(R+) = VREF+; V(R-) = VSS, channel 5, end of sequence
	ADC12IFG = 0;
	ADC12IE = ADC12IE1;									//Interrupt when result loaded into MEM1
	ADC12CTL0 |= ADC12ENC;
	ADC12CTL0 |= ADC12SC;

	//CONFIGURE UART (uncomment if wanting to send data to computer)
	/*
	P2SEL |= BIT4;
	UCA0CTL1 |= UCSWRST;						//USCI disabled
	UCA0CTL0 = 0;								//Optional, for guarantee result
	UCA0CTL1 = UCSSEL_3;						//USCI source: SMCLK = DCOCLKDIV = 10060000 Hz
	UCA0BR0 = 52;								//UCA0BR0 = INT(10060000 / 115200)
	UCA0MCTL = 3 << 1;							//BRSx = round( (87,04 - 87) * 8)
	UCA0CTL1 &= ~UCSWRST;						//USCI enabled
	*/
	
	//CONFIGURE SPI at USCI_A1 for communication with OLED
	UCA1CTL1 |= UCSWRST;						//USCI disabled
	UCA1CTL1 |= UCSSEL_3;						//SMCLK as clock source
	UCA1BR0 = 1;								//SPI clock = SMCLK 
	UCA1BR1 = 0;
	UCA1MCTL = 0;
	UCA1CTL0 |= (UCCKPH | UCMSB | UCMST | UCMODE_1 | UCSYNC);	//Synchronous mode
	P8SEL |= (BIT1 | BIT2 | BIT3);
	UCA1CTL1 &= ~UCSWRST;
	
	//CONFIGURE TIMER
	TA0CTL = TASSEL_1 | TACLR;					//ACLK
	TA0CCTL0 = CCIE;							//Interrupt at the end of timer
	TA0CCTL1 = OUTMOD_3;						//Set_reset mode (ADC12 trigger on rising edge)

	TA0CCR0 = 32-1;								//Sample at 1024Hz for both LEDS
	TA0CCR1 = 18-1;								//Time for PD to stabilize before turning on ADC
	TA0CTL |= MC_1;								//Up mode, timer start counting
}

void ssd1306_draw_plethysmograph(int data)
{
	unsigned char center_page, shifting, i, value = 0;
	static unsigned char k = 0;
	center_page = data / 8;
	shifting = data % 8;
	//For line effect
	for (i=0; i <(7 - shifting) + 1; i++) value = (1<<i);

	ssd1306_set_addressing(VERTICAL_MODE);
	ssd1306_set_column_range(k, k);				//Only one column a time
	ssd1306_set_page_range(3, 7);				//5 byte ~ (0 - 39)
	k++;
	if (k > 127) k = 0;
	
	//Start sending upper value, pixels off
	for (i=3 ; i < (7- center_page); i++) ssd1306_send_data(0x00);

	//Sending line middle value
	ssd1306_send_data(value);

	//For line effect
	for (i= (7 - center_page + 1); i < 8; i++) ssd1306_send_data(0x00);

}
