\ TxbAnsiTerminal.f -- Ansi Terminal Control (subset) - T.Brumley

anew task-TxbAnsiTerminal.fth

\ Colored text in terminals means using ANSI control sequences.
\ While mining some of my Pascal ANSI support I found the best/most
\ readable documentation for ANSI controls:
\
\ https://nicholas-morris.com/articles/ansi-codes
\
\ I've added that to https://vt100.net/ in my bookmarks.
\
\ I only needed to turn red or green text on or off, but I added
\ several sequences I found useful in my Pascal code.
\
\ Notes:
\
\ - Syntax: `char` is `[char]` in compiled code. I found that I
\   could replace this by wrapping a character in apostrophes. I
\   believe that was introduced by Forth-2012.
\
\ - Performance is not an issue, but I wondered about building
\   all these "char emit" chains into string constants. I don't
\   think that would buy me anything.
\
\ TODO:
\
\ - Take a serious look at the string idea above.
\ 
\ - Number to character digit conversion for inserting parameters
\   into several sequences. 

\ See sgr.(f|b)g.* for the 4 bit color mappings and an ability
\ to do rgb coloring.

\ DEC (private) control functions.

: ansi.dec.sc     27 emit '7' emit ; \ save cursor
: ansi.dec.sr     27 emit '8' emit ; \ restore cursor

: ansi.dec.cusor.show 27 emit '?' emit '2' emit '5' emit 'h' emit ;
: ansi.dec.cusor.hide 27 emit '?' emit '2' emit '5' emit 'l' emit ;

: ansi.cursor.home 27 emit 'H' emit ;
: ansi.erase.eos   27 emit 'J' emit ;
: ansi.erase.eol   27 emit 'K' emit ;

\ Control Sequences
\
\ CSI p...p i...i f
\ p...p are parameter bytes, $30-$3f
\ i...i are intermediate bytes, $20-$2f
\ f final byte, $40-$7e ($70-$7e are private)
\
\ so \e[1;3;4;35m means:
\ 
\    \e[       CSI        control sequence introducer
\    1;3;4;35  arguments  bold italic underlined magenta
\    m         function   select graphics rendition

\ Preludes, or "introducers"

: ansi.sgc        27 emit 'Y' emit ;     \ single graphic character introducer
: ansi.sci        27 emit 'Z' emit ;     \ single character introducer
: ansi.csi        27 emit '[' emit ;     \ control sequence introducer

: ansi.delim      ';' emit ;

\ ANSI control fuctions

: ansi.cuu        ansi.csi 'A' emit ;   \ cursor up (def 1)
: ansi.cuu.n  1 abort" NOT IMPLEMENTED" ;
: ansi.cud        ansi.csi 'B' emit ;   \ cursor down (def 1)
: ansi.cud.n  1 abort" NOT IMPLEMENTED" ;
: ansi.cuf        ansi.csi 'C' emit ;   \ cursor forward
: ansi.cub        ansi.csi 'D' emit ;   \ cursor backward
: ansi.cnl        ansi.csi 'E' emit ;   \ cursor next line
: ansi.cpl        ansi.csi 'F' emit ;   \ cursor previous line
\ CSI row ; col H
: ansi.cup    ( row col -- ) 2drop 1 abort " NOT IMPLEMENTED" ;
\ erase screen current to end, beginning to current, or full
: ansi.ed.from    ansi.csi '0' emit 'J' emit ;
: ansi.ed.to      ansi.csi '1' emit 'J' emit ;     
: ansi.ed         ansi.csi '2' emit 'J' emit ;
\ erase line current to end, beginning to current, or full
: ansi.el.from    ansi.csi '0' emit 'K' emit ;
: ansi.el.to      ansi.csi '1' emit 'K' emit ;     
: ansi.el         ansi.csi '2' emit 'K' emit ;

\ Select Graphics Rendition
\
\ Use these to assemble a sequence to modify the text
\ display.
\ 
\ CSI something ; something SGR
\
\ example: `ansi.csi sgr.bright ansi.delim sgr.fg.red ansi.sgr`
 
: ansi.sgr        'm' emit ;       \ oddly enough, the last thing
 
: sgr.default     '0' emit ;
: sgr.bright      '1' emit ;       \ bold/bright/insensified
: sgr.no.bright   '2' emit '2' emit ;
: sgr.underline   '4' ;
: sgr.no.underline '2' emit '4' emit ;
: sgr.reverse     '7' emit ;          \ "negative"
: sgr.no.reverse  '2' emit '7' emit ; \ "positive"
: sgr.italics     '3' emit ;
: sgr.fg.black    '3' emit '0' emit ;
: sgr.fg.red      '3' emit '1' emit ;
: sgr.fg.green    '3' emit '2' emit ;
: sgr.fg.yellow   '3' emit '3' emit ;
: sgr.fg.blue     '3' emit '4' emit ;
: sgr.fg.magenta  '3' emit '5' emit ;
: sgr.fg.cyan     '3' emit '6' emit ;
: sgr.fg.white    '3' emit '7' emit ;
: sgr.fg.extended '3' emit '8' emit ; \ fine grained
: sgr.fg.default  '3' emit '9' emit ; \ only fg defaults
: sgr.bg.black    '4' emit '0' emit ;
: sgr.bg.red      '4' emit '1' emit ;
: sgr.bg.green    '4' emit '2' emit ;
: sgr.bg.yellow   '4' emit '3' emit ;
: sgr.bg.blue     '4' emit '4' emit ;
: sgr.bg.magenta  '4' emit '5' emit ;
: sgr.bg.cyan     '4' emit '6' emit ;
: sgr.bg.white    '4' emit '7' emit ;
: sgr.bg.extended '4' emit '8' emit ; \ fine grained
: sgr.bg.default  '4' emit '9' emit ; \ only bg defaults

\ if needed, 90-99 bright foreground with no extended or default
\           100-109 bright background with no extended or default

\ Extended colors (3|4)8; 2;r;g;b or 5;s
\ need to figure out how to emit multi digit strings
\ : sgr.fg.rgb ( r g b -- ) rot ( g b r ) 
\ : sgr.bg.rgb


\ Things I expect to use often enough to give friendlier names:
\ Change 'ink' colors and provide string prints helpers for
\ command line output. More colors and faces can be added
\ as needed.

: text.red ( -- )
    ansi.csi sgr.fg.red ansi.sgr ;

: text.green ( -- )
    ansi.csi sgr.fg.green ansi.sgr ;

: text.blue ( -- )
    ansi.csi sgr.fg.blue ansi.sgr ;

: text.cyan ( -- )
    ansi.csi sgr.fg.cyan ansi.sgr ;

: text.default ( -- )
    ansi.csi sgr.fg.default ansi.sgr ;
        
: type.red ( str len -- )
    text.red type text.default ;
    
: type.green ( str len -- )
    text.green type text.default ;

: type.blue ( str len -- )
    text.blue type text.default ;

: type.cyan ( str len -- )
    text.cyan type text.default ;

: ctype.red ( cstr -- )
    count type.red ;

: ctype.green ( cstr -- )
    count type.green ;

: ctype.blue ( str len -- )
    count type.blue ;

: ctype.cyan ( str len -- )
    count type.cyan ;


cr
cr c" am i red?" ctype.red
cr c" am i green?" ctype.green
cr s" am i blue?" type.blue
cr s" am i cyan?" type.cyan
cr
 
\ end of TxbAnsiTerminal.fth
