\ test-TxbStrings.fs -- Test string helpers -- T.Brumley

   require TxbUnitTesting.fs
   require TxbStrings.fs

unit-test-reset
\ I'm not worried about move vs cmove vs cmove> semantics.
\ If they're broken, it's nothing I broke.

create S1B8 8 allot
: s1b8 s1b8 8 ;
create S2B16 16 allot
: s2b16 s2b16 16 ;

: reset-safe-move.data ( -- )
   s" aaaaaaaa" s1b8 drop swap move 
   s" bbbbbbbbbbbbbbbb" s2b16 drop swap move ;

reset-safe-move.data

s1b8 s2b16 safe-move
s" aaaaaaaabbbbbbbb" s2b16 compare s" s1b8 to s2b16" 0 unit-test-n
unit-test-stack?

unit-test-report

\ End of test-xbStrings.fs
