@m
D=M
@ZERO
D;JEQ

@k   
M=0
(LOOP)
@n
D=M
@STOP
D;JEQ 
@m
D=D-M
@n
M=D
@STOP
D;JLT
@k
M=M+1
@LOOP
0;JMP

(ZERO)
@k
M=0

(STOP)
@STOP
0;JMP