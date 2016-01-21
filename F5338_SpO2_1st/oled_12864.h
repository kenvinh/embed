/*
 * oled_12864.h
 *
 *  Created on: 14-04-2013
 *      Author: Vinh
 */
#include <msp430.h>
#ifndef OLED_12864_H_
#define OLED_12864_H_

#define ON						1
#define OFF						0

#define OLED_DIR				P9DIR
#define OLED_OUT				P9OUT
#define OLED_RES				BIT0
#define OLED_DC					BIT2
#define OLED_CS					BIT1

#define OLED_WIDTH				128
#define OLED_HEIGHT				64
#define NUM_OF_PAGE				8

#define SMALL_FONT				0x00
#define MEDIUM_FONT				0x01
#define LARGE_FONT				0x02

#define SMALL_FONT_WIDTH		6
#define SMALL_FONT_HEIGHT		8

#define MEDIUM_FONT_WIDTH		14
#define MEDIUM_FONT_HEIGHT		16

#define LARGE_FONT_WIDTH		24	//??
#define LARGE_FONT_HEIGHT		24

//COMMAND TABLE DEFINITION
//1. Fundamental commands
#define	SET_CONTRAST			0x81
#define ENTIRE_DISPLAY_OFF		0xA4
#define	ENTIRE_DISPLAY_ON		0xA5
#define NORMAL_DISPLAY			0xA6
#define INVERSE_DISPLAY			0xA7
#define DISPLAY_ON				0xAF
#define DISPLAY_OFF				0xAE

//2. Scrolling commands
//3. Addressing settings commands
#define SET_MEM_ADDRESSING_MODE	0x20	//Followed by 0x01 - 0x03. 0x01:Horizontal. 0x02:Vertical. 0x03:Page
#define HORIZONTAL_MODE			0x00
#define VERTICAL_MODE			0x01
#define PAGE_MODE				0x02
	//a. For page addressing mode only
#define SET_LOWER_COLUMN		0x00	//OR with 0x00-0x0F. Lower nibble of column start address
#define SET_HIGHER_COLUMN		0x10	//OR with 0x00-0x0F. Higher nibbler of column start address
#define SET_PAGE				0xB0	//OR with 0x00-0x07. Page start address
	//b. For horizontal or vertical addressing mode only
#define SET_COLUMN_RANGE		0x21	//Followed by two bytes, range 0-127. First: column start. Second: column end
#define	SET_PAGE_RANGE			0x22	//Followed by two bytes, range 0-7. First: page start. Second: page end

//4. Hardware configuration commands
#define SET_DISPLAY_START_LINE	0x40	//OR with 0-63. Offset the display
#define	SEGMENT_NO_REMAPPED		0xA0
#define SEGMENT_REMAPPED		0xA1
#define SET_MUX_RATIO			0xA8	//Followed by 15-63. Set MUX ratio, 16MUX - 64MUX
#define COM_NO_REMAPPED			0xC0
#define COM_REMAPPED			0xC8
#define SET_DISPLAY_OFFSET		0xD3	//Followed by 0-63. Offset the display
#define	SET_COM_PINS			0xDA	//Followed by 0x02 or 0x12 or 0x22 or 0x32. ???. Usually 0x12

//5. Timing and driving scheme settings commands
#define SET_OSC_FREQ			0xD5	//Followed by one byte. Lower nibble clock divider. Higher nibble frequency. Default: 0x80
#define SET_PRE_CHARGE			0xD9	//Followed by one byte. Lower nibble phase1 period. Higher nibble phase2 period. Default: 0x22
#define SET_VCOMH_LEVEL			0xDB	//Followed by 0x00 or 0x20 or 0x30 or 0x40. Set detect level of Vcomh. Default: 0x20
#define NOP						0xE3

//6. Charge bump settings commands
#define SET_CHARGE_PUMP			0x8D	//Followed by 0x10 or 0x14. 0x14:Enable charge bump. Display MUST off before this setting.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//FUNCTIONS FOR OLED 128X64
void ssd1306_init(void);
void ssd1306_draw_char(char start_page, char start_column, char character, char font_type);
void ssd1306_draw_image_full(char * image_buffer);
void ssd1306_fill_ram(unsigned char data);
void ssd1306_clear_all(void);
void ssd1306_set_address(char page_start, char column_start);
void ssd1306_set_page_range(char page_start, char page_end);
void ssd1306_set_column_range(char column_start, char column_end);
void ssd1306_set_addressing(unsigned char mode);
void ssd1306_send_command(unsigned char command);
void ssd1306_send_data(unsigned char data);
void ssd1306_send_command_buffer(char * command_buffer, int i);
void ssd1306_send_data_buffer(char * data_buffer, int i);
#endif /* OLED_12864_H_ */
