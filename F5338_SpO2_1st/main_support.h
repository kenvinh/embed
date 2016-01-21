/*
 * main_support.h
 *
 *  Created on: 21-03-2013
 *      Author: Vinh
 */
#include <stdint.h>
#ifndef MAIN_SUPPORT_H_
#define MAIN_SUPPORT_H_

//FIR filter coefficient for removing 50/60Hz and 100/120Hz from the signals

static const long coeffs[12] =
{
		123,
		826,
		1482,
		2654,
		4173,
		6038,
		8143,
		10340,
		12441,
		14248,
		15573,
		16274

};

const unsigned int Lookup [43] = {100, 100, 100, 100, 99, 99, 99, 99, 99, 99, 98, 98, 98, 98,
									  98, 97, 97, 97, 97, 97, 97, 96, 96, 96, 96, 96, 96, 95, 95,
									  95, 95, 95, 95, 94, 94, 94, 94, 94, 93, 93, 93, 93, 93};
//RAW Signal range KEPT between 1.6 and 2.2 V and outer range is 1.2 and 2.4 V. Vref = 2.5V or
//measure for more correct value
#define RAW_SIGNAL_VERY_HIGH    3933			//3933: 2,4V
#define RAW_SIGNAL_HIGH			3605			//3605: 2,2V
#define RAW_SIGNAL_LOW    		2622			//2622: 1,6V
#define RAW_SIGNAL_VERY_LOW    	1967			//1967: 1,2V

#define INTENSITY_MAX			3276
#define INTENSITY_MIN			1

#define SLOW_CORRECTION         1
#define FAST_CORRECTION			5

#define AMPLIFIED_SIGNAL_HIGH	4095
#define AMPLIFIED_SIGNAL_LOW	100

#define DC_OFFSET_MAX			2948
#define DC_OFFSET_MIN			2129

#define NO_OF_HALFCYCLES 				6
#define THRESHOLD				120

//Variables for ADC signal chain of IR led
int ir_raw = 0;									//Read from OPA381
int ir_amplified = 0;							//Read from OPA333
int ir_lowpass_filtered = 0;					//Signal after FIR filter
int ir_ac_level = 0;							//Signal after DC extraction
//int ir_dc_level = 0;
//int ir_ac_level_2 = 0;
unsigned long sq_ir_ac_level, sq_red_ac_level;
unsigned long log_sq_ir_ac_level, log_sq_red_ac_level;
unsigned long x, y;
unsigned int ratio, SaO2;
//long ir_dc_register2 = 0;
unsigned int heart_signal_sample_counter = 0;
unsigned char rising_edge = 0, debounce_counter = 0;
//unsigned char falling_edge = 0;
unsigned int heart_beat_halfcycle_count, log_heart_signal_sample_counter;
unsigned int heart_rate, heart_rate_mean;
unsigned char heart_rate_buf[10];
long ir_dc_register = 0;
//unsigned int buf_ir_amplified[2000];
//int buf_ir_ac_level_2[2000];
//int buf_ir_ac_level[2000];
int buf_scaled_ir[2000];
unsigned int buf_ir_count = 0;
int prev_ir_ac_level;
//int prev_ir_ac_level_2;

//Variables for ADC signal chain of RED led
int red_raw = 0, red_amplified = 0, red_lowpass_filtered = 0, red_ac_level = 0;
long red_dc_register = 0;

//Variables for DAC0 => H-bridge
unsigned int ir_led_intensity = 1800, red_led_intensity = 2000;

//Variables for DAC1 => DC offset for OPA333
unsigned int ir_dc_offset = 2000, red_dc_offset = 2000;

unsigned char a, b, c, d, e, f;
int scaled_ir_ac_level_2;
unsigned char draw_skip_count = 0;
unsigned char is_IR = 0;
unsigned char finger_in = 0, finger_previous_state = 1;

int ir_filter(int sample);
int red_filter(int sample);
int dc_extraction(long *p, int x);
void configure_system(void);
void ssd1306_draw_plethysmograph(int data);
#endif /* MAIN_SUPPORT_H_ */
