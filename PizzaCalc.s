.text
.align 2

main:
    addi $sp, $sp, -4
    sw $ra, 0($sp) #save ra

    #build linkedList
    jal newNode
    move $s0, $v0 #save head in s0
    move $s1, $v0 #s1 will be "current"
    beqz $v0, _emptyfile

_loop:
    jal newNode
    beqz $v0, _endloop #branch if newNode returns 0
    sw $v0, 68($s1) #current.next = newNode
    lw $s1, 68($s1) #current = current.next
    j _loop

_endloop:
    sw $0, 68($s1) #s1.next = null, this is last node

    move $s1, $s0 #set current = to head and prepare sort
    
    move $a0, $s1
    jal sortNodes
    #testing testing
    #
    #end main
    move $s1, $v0
    la $s3, NEWLINE
    lb $s4, 0($s3)

_loopprint:

    la $a0, 0($s1)
_walkNewline:
    lb $s2, 0($a0)
    beq $s2, $s4, _trimNewline #walk the string until you find newline, replace with 0
    addi $a0, $a0, 1
    j _walkNewline
_trimNewline:
    sb $0, 0($a0)
    la $a0, 0($s1)
    li $v0, 4
    syscall

    la $a0, SPACE
    li $v0, 4 #print space
    syscall

    l.s $f12, 64($s1)
    li $v0, 2
    syscall

    la $a0, NEWLINE
    li $v0, 4
    syscall

    lw $s1, 68($s1)
    bnez $s1, _loopprint
    
    lw $ra, 0($sp) #restore ra
    addi $sp, $sp, 4
    jr $ra

_emptyfile:
    li $v0, 4
    la $a0, EMPTY
    syscall
    lw $ra, 0($sp) #restore ra
    addi $sp, $sp, 4
    jr $ra

sortNodes:
    addi $sp, $sp, -36
    sw $ra, 32($sp)
    sw $s0, 28($sp)
    sw $s1, 24($sp)
    sw $s2, 20($sp)
    sw $s3, 16($sp)
    sw $s4, 12($sp)
    sw $s5, 8($sp)
    s.s $f20, 4($sp)
    s.s $f21, 0($sp)
    #save s, f, and ra
    lw $s0, 68($a0) # end if next = null, s0 will be current
    beqz $s0, _endsort
    move $s1, $a0 #previous
    move $s2, $a0 #head

_do:
    li $s3, 0 #swaps
    #if current greater than previous
    l.s $f20, 64($s0) #f20 is current pizzapernode
    l.s $f21, 64($s1) #f21 is previous pizzerpernode

    c.le.s $f20, $f21 #if(current->pizzaPerDollar > previous -> pizzaPerDollar){
    bc1f _swap
    c.eq.s $f20, $f21 #if current 0>pizzaperdollar == previous -> pizzaperdollar)
    bc1t _ppdSame

_while:
    beqz $s0, _endWhile #while (current != NULL && current->next != NULL){
    lw $s5, 68($s0) #s5 is current.next
    beqz $s5, _endWhile
    l.s $f20, 64($s5) #f20 is current->next ->pizzaperdollar
    l.s $f21, 64($s0)  #f21 is current -> pizzaPerDollar

    c.le.s $f20, $f21 #if (current->next->pizzaPerDollar > current->pizzaPerDollar)
    bc1f _swap2
    c.eq.s $f20, $f21 #current->next->pizzaPerDollar == current->pizzaPerDollar
    bc1t _ppdSame2

_advance:
    move $s1, $s0#previous = current;
    lw $s0, 68($s0)#current = current->next;
    j _while
    
_endWhile:
    move $s1, $s2 #previous = headNode;
    lw $s0, 68($s2) #current = headNode->next;
    bgtz $s3, _do #if swaps > 0
    move $v0, $s2 #return the headnode
    l.s $f21, 0($sp)
    l.s $f20, 4($sp)
    lw $s5, 8($sp)
    lw $s4, 12($sp)
    lw $s3, 16($sp)
    lw $s2, 20($sp)
    lw $s1, 24($sp)
    lw $s0, 28($sp)
    lw $ra, 32($sp)
    addi $sp, $sp, 36
    jr $ra

    #else if(current -> pizzaPerDollar == previous -> pizzaPerDollar){
_ppdSame:
    la $a0, 0($s0)
    la $a1, 0($s1)
    jal strcmp
    bltz $v0, _swap #swap if previous name is > than current name\\\\
    j _while
_swap:
    move $s4, $s0 #temp = current
    lw $s5, 68($s0)
    sw $s5, 68($s1) #previous -> next = current -> next;
    sw $s1, 68($s0) #current -> next = previous;
    move $s2, $s0 #headNode = current
    move $s0, $s1 #current = previous
    move $s1, $s2 #previous = headnode
    li $s3, 1 #swaps = 1
    j _while
_ppdSame2:
    la $a0, 0($s0)
    la $a1, 0($s5)
    jal strcmp
    bgtz $v0, _swap2
    j _advance
_swap2:
    move $s4, $s5#struct PizzaNode* temp = current->next;
    lw $s5, 68($s5)
    sw $s5, 68($s0)#current->next = current->next->next;
    #lw $s5, 68($s5)
    sw $s0, 68($s4)#temp -> next = current;
    sw $s4, 68($s1)#previous -> next = temp;
    li $s3, 1#swaps = 1;         
    j _advance
_endsort:
    l.s $f21, 0($sp)
    l.s $f20, 4($sp)
    lw $s5, 8($sp)
    lw $s4, 12($sp)
    lw $s3, 16($sp)
    lw $s2, 20($sp)
    lw $s1, 24($sp)
    lw $s0, 28($sp)
    lw $ra, 32($sp)
    addi $sp, $sp, 36
    move $v0, $a0
    jr $ra
                


newNode:
#SAVE AND RESTORE s0 AND RA
    addi $sp, $sp, -8
    sw $ra, 4($sp)
    sw $s0, 0($sp)

    li $v0, 9
    li $a0, 72
    syscall #malloc for the struct

    move $s0, $v0 #struct in s0

    li $v0, 4
    la $a0, promptName #print prompt for name
    syscall

    li $v0, 8
    la $a0, 0($s0)
    li $a1, 64 #read string input
    syscall

    #compare to DONE
    la $a0, 0($s0)
    la $a1, DONE
    
    jal strcmp 

    beqz $v0, _endList
    
    li $v0, 4
    la $a0, promptDiameter #print prompt for diameter
    syscall

    li $v0, 6
    syscall #read float
    mov.s $f12, $f0 #store in f12

    li $v0, 4
    la $a0, promptCost #print prompt for cost
    syscall

    li $v0, 6
    syscall #read float
    mov.s $f13, $f0 #store in f13
    #f1 diameter $f2 cost

    jal pizzaPerDollar
    s.s $f0, 64($s0) #storing pizzaperdollar

    move $v0, $s0 #return the struct in v0
    #SAVE AND RESTORE JR AND RA
    lw $s0, 0($sp)
    lw $ra, 4($sp)
    addi $sp, $sp, 8
    jr $ra

_endList:
    li $v1, 0
    lw $s0, 0($sp)
    lw $ra, 4($sp)
    addi $sp, $sp, 8
    jr $ra

pizzaPerDollar:
    #add fxn for 0
    l.s $f4, PI
    l.s $f5, TWO
    l.s $f6, ZERO
    c.eq.s $f6, $f13
    bc1t _zeroReturn
    
    div.s $f12, $f12, $f5 #f12 has radius
    mul.s $f12, $f12, $f12, #f12 has radius squared
    mul.s $f12, $f12, $f4 #$f12 has area
    div.s $f0, $f12, $f13 #f0 has pizzaperdolla
    jr $ra
_zeroReturn:
    mov.s $f0, $f6
    jr $ra



strcmp:

_strcmp_loop:
    lb $t0, 0($a0) #load first chars from a0 and a1 strings
    lb $t1, 0($a1)
    bne	$t0, $t1, _strcmp_endloop	# if $t0 != $t1 then target
    beqz $t0, _strcmp_endloop #if null, end loop
    addi $a0, $a0, 1 #walk one
    addi $a1, $a1, 1
    j _strcmp_loop

_strcmp_endloop:
    beq	$t0, $t1, _strcmp_returnequal
    sub $t2, $t0, $t1 #return the result of t1 compared to t0
    move $v0, $t2
    jr $ra
_strcmp_returnequal:
    li $v0, 0
    jr $ra





.data

promptName: .asciiz "Pizza name:"
promptDiameter: .asciiz "Pizza diameter:"
promptCost: .asciiz "Pizza cost:"
buffer: .space 64
PI: .float 3.14159265358979323846 
TWO: .float 2.0
ZERO: .float 0.0
DONE: .asciiz "DONE\n"
EMPTY: .asciiz "PIZZA FILE IS EMPTY"
NEWLINE: .asciiz "\n"
SPACE: .asciiz " "