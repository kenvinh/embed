/*
 * oled_12864.c
 *
 *  Created on: 14-04-2013
 *      Author: Vinh
 */
#include <msp430.h>
#include "oled_12864.h"
#include "delay.h"
#include "fontsmall_6x8.h"
#include "fontmedium_14x16.h"

void ssd1306_init(void)
{
	//Configure port pin direction for CS, RES, DC pin
	OLED_OUT &= ~(OLED_RES | OLED_DC);
	OLED_OUT |= OLED_CS;
	OLED_DIR |= (OLED_CS | OLED_RES | OLED_DC);	//Output direction
	DELAY_US(200000);							//Wait OLED 200ms for initialization

	//Now, pull OLED_RES high
	OLED_OUT |= OLED_RES;

	//Start sending command
   ssd1306_send_command(DISPLAY_OFF);			//--turn off oled panel
   ssd1306_send_command(0x00);					//---set low column address
   ssd1306_send_command(0x10);					//---set high column address
   ssd1306_send_command(0x40);					//--set start line address
   ssd1306_send_command(SET_CONTRAST);			//--set contrast control register
   ssd1306_send_command(0x7f);
   ssd1306_send_command(SEGMENT_REMAPPED);		//--set segment re-map 95 to 0
   ssd1306_send_command(COM_REMAPPED);			//--set segment re-map 95 to 0
   ssd1306_send_command(NORMAL_DISPLAY);		//--set normal display
   ssd1306_send_command(SET_MUX_RATIO);			//--set multiplex ratio(1 to 64)
   ssd1306_send_command(63);					//--1/64 duty
   ssd1306_send_command(SET_DISPLAY_OFFSET);	//-set display offset
   ssd1306_send_command(0x00);					//-not offset
   ssd1306_send_command(SET_OSC_FREQ);			//--set display clock divide ratio/oscillator frequency
   ssd1306_send_command(0xF0);					//--set divide ratio
   ssd1306_send_command(SET_PRE_CHARGE);		//--set pre-charge period
   ssd1306_send_command(0xF1);
   ssd1306_send_command(SET_COM_PINS);			//--set com pins hardware configuration
   ssd1306_send_command(0x12);
   ssd1306_send_command(SET_VCOMH_LEVEL);		//--set vcomh
   ssd1306_send_command(0x20);
   ssd1306_send_command(SET_CHARGE_PUMP);		//--set Charge Pump enable/disable
   ssd1306_send_command(0x14);					//--set(0x10) disable
   ssd1306_send_command(DISPLAY_ON);			//--turn on oled panel
   ssd1306_fill_ram(0x00);								// Clear Screen
}

void ssd1306_draw_char(char start_page, char start_column, char character, char font_type)
{
	unsigned int temp;
	ssd1306_set_addressing(VERTICAL_MODE);
	ssd1306_set_page_range(start_page, start_page + font_type);
	ssd1306_set_column_range(start_column, 127);
	if (character < 32 || character > 129)
		character = '.';
	switch(font_type)
	{
	case SMALL_FONT:
		temp = (character - 32) * SMALL_FONT_WIDTH;
		ssd1306_send_data_buffer((char *)font_small + temp, 6);			//Send 6 bytes of each character for small font
		break;
	case MEDIUM_FONT:
		temp = (character - 32) * MEDIUM_FONT_WIDTH * 2;
		ssd1306_send_data_buffer((char *)font_medium + temp, 28);
		break;
	//case LARGE_FONT:
	//	temp = (character - 32) * FONT_SMALL_WIDTH;
	//	ssd1306_send_data_buffer((char *)font_small + temp, 6);
	//	break;
	}
}

void ssd1306_draw_image_full(char * image_buffer)	//This function intended for horizontal writing mode
{
	ssd1306_set_addressing(HORIZONTAL_MODE);
	ssd1306_set_page_range(0,7);
	ssd1306_set_column_range(0,127);
	ssd1306_send_data_buffer((char *)image_buffer, 1024);
}

void ssd1306_fill_ram(unsigned char data)
{
	unsigned int i;
	ssd1306_set_addressing(HORIZONTAL_MODE);
	ssd1306_set_page_range(0,7);
	ssd1306_set_column_range(0,127);
	for (i=0; i < 8192; i++)
	{
		ssd1306_send_data(data);
	}
}

void ssd1306_clear_all(void)
{
	ssd1306_fill_ram(0x00);
}

//Set address:
//For page addressing mode
void ssd1306_set_address(char page_start, char column_start)
{
	char byte_to_send[3];
	byte_to_send[0]= SET_PAGE | page_start;
	byte_to_send[1]= SET_LOWER_COLUMN | (column_start & 0x0F);
	byte_to_send[2]= SET_HIGHER_COLUMN | ( (column_start & 0xF0) >> 4 );
	ssd1306_send_command_buffer(byte_to_send,3);
}

//Set page range, for horizontal and vertical mode
void ssd1306_set_page_range(char page_start, char page_end)
{
	char byte_to_send[3];
	byte_to_send[0] = SET_PAGE_RANGE;
	byte_to_send[1] = page_start;
	byte_to_send[2] = page_end;
	ssd1306_send_command_buffer(byte_to_send, 3);
}
//Set column range, for horizontal and vertical mode
void ssd1306_set_column_range(char column_start, char column_end)
{
	char byte_to_send[3];
	byte_to_send[0] = SET_COLUMN_RANGE;
	byte_to_send[1] = column_start;
	byte_to_send[2] = column_end;
	ssd1306_send_command_buffer(byte_to_send, 3);
}

void ssd1306_set_addressing(unsigned char mode)
{
	char buffer[2];
	buffer[0] = SET_MEM_ADDRESSING_MODE;
	buffer[1] = mode;
	ssd1306_send_command_buffer(buffer, 2);
}

void ssd1306_send_command_buffer(char * command_buffer, int i)
{
	volatile unsigned char temp;
	OLED_OUT |= OLED_CS;
	OLED_OUT &= ~OLED_DC;
	OLED_OUT &= ~OLED_CS;
	while(i)
	{
		while(!(UCA1IFG & UCTXIFG));
		UCA1TXBUF = *command_buffer;
		command_buffer++;
		i--;
	}
	while(UCA1STAT & UCBUSY);
	temp = UCA1RXBUF;
	OLED_OUT |= OLED_CS;
}

void ssd1306_send_data_buffer(char * data_buffer, int i)
{
	volatile unsigned char temp;
	OLED_OUT |= OLED_CS;
	OLED_OUT |= OLED_DC;
	OLED_OUT &= ~OLED_CS;
	while(i)
	{
		while(!(UCA1IFG & UCTXIFG));
		UCA1TXBUF = *data_buffer;
		data_buffer++;
		i--;
	}
	while(UCA1STAT & UCBUSY);
	temp = UCA1RXBUF;
	OLED_OUT |= OLED_CS;
}

void ssd1306_send_command(unsigned char command)
{
	volatile unsigned char temp;
	OLED_OUT |= OLED_CS;
	OLED_OUT &= ~OLED_DC;
	OLED_OUT &= ~OLED_CS;
	while(!(UCA1IFG & UCTXIFG));
	UCA1TXBUF = command;
	while(UCA1STAT & UCBUSY);
	temp = UCA1RXBUF;
	OLED_OUT |= OLED_CS;
}

void ssd1306_send_data(unsigned char data)
{
	volatile unsigned char temp;
	OLED_OUT |= OLED_CS;
	OLED_OUT |= OLED_DC;
	OLED_OUT &= ~OLED_CS;
	while(!(UCA1IFG & UCTXIFG));
	UCA1TXBUF = data;
	while(UCA1STAT & UCBUSY);
	temp = UCA1RXBUF;
	OLED_OUT |= OLED_CS;
}
