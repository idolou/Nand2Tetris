@balanced
M=0

@sum_left
M=0

@sum_right
M=0

@j
M=0

@i
M=0

@k   
M=0

@n
D=M
@n_div
M=D

(loop_0)
@n_div
D=M
@STOP_DIV
D;JEQ 
@m
M=1
@m
M=M+1
@m
D=D-M
@n_div
M=D
@STOP_DIV
D;JLT
@k
M=M+1
@loop_0
0;JMP

(STOP_DIV)



(loop_1)
@j
D=M
@k
D=D-M
@STOP_LEFT
D;JGE

@array
D=M
@j
A=D+M
D=M
@sum_left
M=M+D
@j
M=M+1
@loop_1
0;JMP

(STOP_LEFT)




(loop_2)
@k
D=M
@n
D=D-M
@STOP_RIGHT
D;JGE

@array
D=M
@k
A=D+M
D=M
@sum_right
M=M+D
@k
M=M+1
@loop_2
0;JMP

(STOP_RIGHT)

@sum_left
D=M
@sum_right
D=D-M
@END
D;JNE

@balanced
M=1

(END)
@END
0;JMP