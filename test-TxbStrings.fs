\ test-TxbStrings.fth -- Test string helpers -- T.Brumley

   MARKER TEST-TXBSTRINGS

   REQUIRE TxbUnitTesting.fth
   REQUIRE TxbStrings.fth

unit.test.reset
\ I'm not worried about move vs cmove vs cmove> semantics.
\ If they're broken, it's nothing I broke.

CREATE S1B8 8 ALLOT
: s1b8 s1b8 8 ;
CREATE S2B16 16 ALLOT
: s2b16 s2b16 16 ;

: RESET.SAFE.MOVE.DATA ( -- )
   s" aaaaaaaa" s1b8 drop swap move 
   s" bbbbbbbbbbbbbbbb" s2b16 drop swap move ;

reset.safe.move.data

s1b8 s2b16 safe.move
s" aaaaaaaabbbbbbbb" s2b16 compare s" s1b8 to s2b16" 0 unit.test.n
unit.test.stack?

unit.test.report

\ End of test-xbStrings.fth
