#ifndef _SIMPLE_DELAY_LIB_H
#define _SIMPLE_DELAY_LIB_H
/*****************************************************************************
 * Provide short and simple delay function for communication with other device
 * For up to ms delay, write a timer fucntion for it
 * Only change CYCLES_1US definition according to MCLK
 ****************************************************************************/
#define CYCLES_1US		16
#define DELAY_US(x)		__delay_cycles(x * CYCLES_1US)

#endif
