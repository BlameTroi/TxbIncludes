\ TxbWords.fs -- My common definitions -- T.Brumley

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

\ Definitions in this file:
\
\    3DUP                ( n1 n2 n3 -- n1 n2 n3 n1 n2 n3 ) 
\    CHAR+               ( n -- n+1 )
\    CHARS               ( n -- n )
\    NOT                *( n -- f , 0= of n )
\    STACK-DEPTH-TRACER  ( s-string -- )
\    WITHIN?             ( n lo hi -- f , >=lo & <= hi )
\    RANDOM              ( -- n , a random number )
\    CHOOSE              ( n -- r , 0 <= r < n )
\    TEXT                ( c -- , accept to c, copy to PAD )
\    -TEXT               ( a-addr1 u a-addr2 -- f , by cells )
\    READ-LINE-SKIP-EMPTY  ( read-line that eats empty lines )

\ * Using NOT is a bad because some Forths define it as INVERT
\ while others define it as 0=. When I think NOT I think 0=.
\ If NOT is defined, override it to display a warning as it
\ executes the implementation's NOT. This should be a no-op as
\ it won't show for prior definitions and I don't plan on using
\ NOT in my code.

[DEFINED] not [IF]
cr
." Danger! NOT is defined in this Forth!"
cr
: not
   cr ." DANGER! Don't use NOT in your code!" cr
   not ;
[THEN]

\ Add a closed interval range check. I can't come up with a
\ consistent naming using ( and ] that won't be confused with
\ compile time vs interpretation time use of [].
\ 
\ ANS WITHIN is n x y -- flag , x <= n < y, or [x, y )
\ 
\ I have seen n x y 1+ WITHIN in some code, but I prefer this
\ definition from _Programming Forth_:

[UNDEFINED] within? [IF]
: within? ( n low high -- flag , closed interval )
   1+ within ;
[THEN]

[UNDEFINED] 3dup [IF]
: 3dup ( n1 n2 n3 -- n1 n2 n3 n1 n2 n3 )
   dup >r -rot
   dup >r -rot
   dup >r -rot
   r>
   r>
   r> ;
[THEN]

\ This is a handy helper when I've dropped a stack entry.

[DEFINED] stack.depth.tracer [IF]
cr ." Warning! Redefining STACK-DEPTH-TRACER!" cr
[THEN]

: stack-depth-tracer ( s-string -- , print string and depth )
   cr type depth . cr ;

\ CHARS and CHAR+ should be defined already. These assume a
\ character is 1 byte unaligned.

[UNDEFINED] chars [IF]
: chars ; ( c-addr1 -- c-addr1 , no change )
[THEN]

[UNDEFINED] char+ [IF]
: char+ ( c-addr1 -- caddr2 , add one )
   1+ ;
[THEN]

\ A random number generator (16-bit) from Starting Forth.
\
\ The RANDOM and CHOOSE in pforth use this algorithm but for
\ 64-bit instead of 32-bit.
\
\ For some reason I went for a 24-bit range instead of the full
\ 64. I don't remember why but it works.

[UNDEFINED] random [IF]
\ TODO: adjust random-mask as a value.

variable random-seed   here random-seed !
16777215 constant random-mask   ( 24 bit )

: random ( -- u )
   random-seed @ 31421 * 6927 +
   random-mask and dup random-seed !   ;

: choose ( u1 -- u2 , 0 <= u1 < u1 )
   random-mask swap /           \ scale
   random swap / ;
[THEN]

\ TEXT is an old word not in the new standard. I'm not sure why
\ it was dropped, but it could be because definitions use the
\ PAD and the standard is that no standard definitions may
\ change the PAD.
\
\ The PAD is must be at least 84 bytes in length and so this
\ version of TEXT will not read anything longer than 84 bytes.
\
\ WORD's buffer must be at least 33 bytes in length. One
\ length byte followed by room for at least 32 characters.

[UNDEFINED] text [IF]
84 value pad-usable-len                 \ 84 is a minimum
: text ( c -- , delimiter for word )
   word count pad-usable-len min        \ c-addr u
   pad dup pad-usable-len erase swap move ; \ move to pad 
[THEN]

\ `-TEXT` is another old word from Starting Forth. It returns
\ false if two strings are equal, < 0 for first less than
\ second, > 0 for second less than first. This is a bit like
\ the C library cmp interface.
\
\ I don't expect to use this much, it's counts on aligned
\ strings that are an even number of cells long and does a cell
\ by cell compare.

[UNDEFINED] -text [IF]
: -text ( a-addr1 u a-addr2 -- flag )
   2dup + swap
   do
      drop cell+
      dup cell- @ i @ - dup
      if dup abs / leave then
   cell +loop
   swap drop ;
[THEN]

\ This is a version of read-line that eats blank lines. Its
\ interface is the same as read-line.

: read-line-skip-empty ( c-addr u1 fd -- u2 flag ior )
  { ubuf ulen ufd }
  begin
    ubuf ulen ufd read-line { rlen rgot rior }
          rior if true else
       rgot 0= if true else
               rlen then then
  until
  rlen rgot rior ;

\ End of TxbWords.fs
