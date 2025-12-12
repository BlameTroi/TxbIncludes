\ TxbWords.fth -- My common definitions -- T.Brumley

\ Definitions I have feel are worth having available in my
\ code. They may already exist, been borrowed from code I've
\ found, or do things "my" way. They don't fall into a more
\ focusd category such as "unit test" or "strings."
\
\ Some Forths have some of these words so their definitions
\ are guarded. If an existing definition exists, a warning
\ is printed and the (when appropriate) my definition is
\ used as an override. See each definition below for more
\ details.
\
\ For example see the case of NOT as it relates to INVERT
\ and 0=. The standard allows NOT to be a synonym of either
\ 0= or INVERT. This is a bad thing. A Forth's documentation
\ is required to tell you which definition of NOT it uses,
\ but it's better not to use NOT.

   MARKER TXBWORDS

\ Definitions in this file:
\
\    3DUP                ( n1 n2 n3 -- n1 n2 n3 n1 n2 n3 ) 
\    CHAR+               ( n -- n+1 )
\    CHARS               ( n -- n )
\    NOT                *( n -- f , 0= of n )
\    STACK.DEPTH.TRACE   ( s-string -- )
\    WITHIN?             ( n lo hi -- f , >=lo & <= hi )
    
\ Using NOT is a bad because some forths define it as INVERT
\ while others define it as 0=. When I think NOT I think 0=.
\ If NOT is defined, override it to display a warning as it
\ executes the implementation's NOT.

[DEFINED] NOT [IF]
   cr
   ." Danger! NOT is defined in this Forth. Adding warning."
   cr
   : NOT
      cr ." DANGER! Don't use NOT in your code!" cr
      NOT ; 
[THEN]

\ Add a closed interval range check. I can't come up with
\ a consistent naming using ( and ] that won't be confused
\ with compile time vs interpretation time use of [].
\ 
\ ANS WITHIN is n x y -- flag , x <= n < y, or [x, y )
\ 
\ I have seen n x y 1+ WITHIN in some code, but I prefer
\ this definition from _Programming Forth_:

[DEFINED] WITHIN? 0= [IF]
   : WITHIN? ( n low high -- flag , closed interval )
      1+ within ;
[ELSE]
   cr ." Using existing definition of WITHIN?." cr
[THEN]

[DEFINED] 3DUP 0= [IF]
: 3dup ( n1 n2 n3 -- n1 n2 n3 n1 n2 n3 )
   dup >r -rot
   dup >r -rot
   dup >r -rot
   r>
   r>
   r> ;
[ELSE]
   cr ." Using existing definition of 3DUP." cr
[THEN]

\ This is a handy helper when I've dropped a stack entry.

[DEFINED] STACK.DEPTH.TRACE [IF]
   cr ." Redefining STACK.DEPTH.TRACE!" cr
[THEN]
: STACK.DEPTH.TRACE ( s-string -- , print string and depth )
   cr type depth . cr ;

\ CHARS and CHAR+ should be defined already. These assume a
\ character is 1 byte unaligned.

[DEFINED] CHARS 0= [IF]
   cr ." CHARS not defined, defaulting to 1*." cr
   : CHARS ; ( c-addr1 -- c-addr1 , no change )
[THEN]

[DEFINED] CHAR+ 0= [IF]
   cr ." CHAR+ not defined, defaulting to 1+." cr
   : CHAR+ ( c-addr1 -- caddr2 , add one )
      1+ ;
[THEN]

\ End of TxbWords.fth
