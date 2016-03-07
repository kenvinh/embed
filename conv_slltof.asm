.PUBLIC _CONV_slltof

_CONV_slltof:
	;Stack allocation:
	; AX: f_output pointer
	; BC: sl_input pointer
	PUSH	HL
	PUSH	DE
	PUSH	AX
	PUSH	BC
	
	;HL now is sl_input
	;
	MOVW	AX, BC
	;ADDW	AX, #0x04
	MOVW	HL, AX
	
	;Check if input is negative or not
	MOVW	AX, #0x8000
	AND		A, [HL+0x07]
	BZ		$_CONV_SLL_Convert

_CONV_SLL_Rev:
	;sl_input = -sl_input
	CLRW	AX
	SUBW	AX, [HL]
	MOVW	[HL], AX
	
	CLRW	AX
	SUBC	A, [HL+0x02]
	MOV		[HL+0x02], A
	XCH		A, X
	SUBC	A, [HL+0x03]
	MOV		[HL+0x03], A
	
	CLRW	AX
	SUBC	A, [HL+0x04]
	MOV		[HL+0x04], A
	XCH		A, X
	SUBC	A, [HL+0x05]
	MOV		[HL+0x05], A
	
	CLRW	AX
	SUBC	A, [HL+0x06]
	MOV		[HL+0x06], A
	XCH		A, X
	SUBC	A, [HL+0x07]
	MOV		[HL+0x07], A	

_CONV_SLL_Convert:
	
	
_CONV_SLL_End:
	POP		BC
	POP		AX
	POP		DE
	POP		HL
	RET
	

	