\ TxbStrings.fs -- My string helpers -- T.Brumley

   marker TXBSTRINGS

   require TxbWords.fs
   require TxbUnitTesting.fs

\ Strings are character aligned but create should always
\ align data space storage. VARIABLE could be used but it
\ allocates one cell. I prefer:
\
\   create somestring 16 CHARS ALLOT
\
\ to:
\
\   VARIABLE somestring 8 CHARS ALLOT
\
\ As the intended length is more clear.
\ 
\ From reading the standard counted strings are "old style"
\ and since Forth 94 strings on the stack are expected to
\ be c-addr u. I'll avoid C" in my new code.
\ 
\ Anytime I want a string I almost always want its length.
\
\ This leads to the following coding patterns:
\
\ create var n allot
\ : var ( -- c-addr u , var address and alloted length )
\    var n ;
\ 
\ var BLANK ( set to blanks )
\ var ERASE ( set to $00 )
\
\ Given strings s1 and s2, where length s1 <= length s2 and
\ the shawoing behavior above:
\
\ Move s1 to s2 from low address to high for the length
\ of s1, and the rest of s2 is unchanged. ( MVC S2,S1 ): 
\ 
\ s1       ( -- @s1 l1 )
\ s2       ( -- @s1 l1 @s2 l2 )
\ DROP     ( -- @s1 l1 @s2 )
\ SWAP     ( -- @s1 @s2 u1)
\ MOVE     ( -- , or cmove or cmove>, see following )
\
\ Or more succinctly: 
\ 
\ s1 s2 DROP SWAP CMOVE
\
\ CMOVE and CMOVE> allow for propogation as an S360 MVC.
\ The result of MOVE is as if s1 -> temp buffer -> s2. This
\ is consistent with memmove() from clib.
\
\ Safe moves do not overflow destination fields nor do they
\ modify trailing bytes of the destination of the source
\ length is less than the destination length.
\
\ I will use MOVE until I see a need for CMOVE and CMOVE>.

\ safe-move copies up to u2 characters from addr1 to addr2.
\ If u1 < u2, only u1 characters are copied. If u2 > u1,
\ only u1 characters are copied and the remaining portion
\ at addr2 is left unchanged.

: safe-move ( c-addr1 u1 c-addr2 u2 -- )
   2dup 2>r       ( s: a1 u1 a2 u2       r: a2 u2 )
   swap drop 2dup ( s: a1 u1 u2 u1 u2    r: a2 u2 )
   > if           ( s: a1 u1 u2 u1>u2    r: a2 u2 )
      swap drop   ( s: a1 u2             r: a2 u2 )
   else
      drop        ( s: a1 u1             r: a2 u2 )
   then
   2r>
   cr .s cr
   drop swap      ( s: a1 u? a2 )
   move ;

\ End of TxbStrings.fs
