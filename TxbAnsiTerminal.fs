\ TxbAnsiTerminal.fs -- Ansi Terminal Control -- T.Brumley

   marker TXBANSITERMINAL

\ Colored text in terminals means using ANSI control
\ sequences. While mining some of my Pascal ANSI support
\ code I found the best/most readable documentation for
\ ANSI controls at:
\
\ https://nicholas-morris.com/articles/ansi-codes
\
\ I've added that to https://vt100.net/ in my bookmarks.
\
\ I only needed to turn red or green text on or off, but I
\ added several sequences I found useful in my Pascal code.
\
\ Notes:
\
\ - Syntax: CHAR is [CHAR] in compiled code. I found that
\   I could replace this by wrapping a character with
\   apostrophes.
\
\ - Performance is not an issue, but I wondered about
\   building all these "char emit" chains into string
\   constants. I don't think that would buy me anything.
\
\ TODO:
\
\ - Take a serious look at the string idea above.
\
\ - Number to character digit conversion for inserting
\   parameters into several sequences.

\ These are control or directive functions. While the
\ distinction appears to be meaningless today, some of
\ them were documented as being "private" or "DEC"
\ specific.
\
\ These words expect nothing on the stack.

: ansi-cursor-save     \ push cursor
   27 emit '7' emit ;

: ansi-cursor-restore  \ pop cursor
   27 emit '8' emit ;

: ansi-cursor-show
   27 emit '?' emit '2' emit '5' emit 'h' emit ;

: ansi-cursor-hide
   27 emit '?' emit '2' emit '5' emit 'l' emit ;

: ansi-cursor-home     \ cursor to 1,1
   27 emit 'H' emit ;

: ansi-erase-eos       \ from cursor to end of screen
   27 emit 'J' emit ;

: ansi-erase-eol       \ from cursor to end of line
   27 emit 'K' emit ;

\ Control Sequences
\
\ The general format is:
\
\ introducer  p...p i...i f
\
\ Where:
\
\ introducer is one of SGC, SCI, or CSI.
\ p...p      are parameter bytes, $30-$3f
\ i...i      are intermediate bytes, $20-$2f
\ f          final byte, $40-$7e ($70-$7e are private)
\
\ Multiple parameter or intermediate items are delimited by
\ semicolons.
\
\ so \e[1;3;4;35m is:
\
\    \e[       CSI        control sequence introducer
\    1;3;4;35  arguments  bold italic underlined magenta
\    m         function   select graphics rendition
\
\ So far only the sequenced that don't require numeric
\ parameters have been implemented.
\
\ These words expect nothing on the stack.

\ Markers and delimiters.

: ansi-sgc             \ single graphic character introducer
   27 emit 'Y' emit ;

: ansi-sci             \ single character introducer
   27 emit 'Z' emit ;

: ansi-csi             \ control sequence introducer        
   27 emit '[' emit ;

: ansi-delim           \ gratuitous factoring
   ';' emit ;


\ ANSI control fuctions
\
\ These move the cursor- Some could take counts but I have
\ not implemented them yet.

: ansi-cuu
   ansi-csi 'A' emit ;   \ cursor up (def 1)
: ansi-cuu-n
   1 abort" NOT IMPLEMENTED" ;
: ansi-cud
   ansi-csi 'B' emit ;   \ cursor down (def 1)
: ansi-cud-n
   1 abort" NOT IMPLEMENTED" ;
: ansi-cuf
   ansi-csi 'C' emit ;   \ cursor forward
: ansi-cub
   ansi-csi 'D' emit ;   \ cursor backward
: ansi-cnl
   ansi-csi 'E' emit ;   \ cursor next line
: ansi-cpl
   ansi-csi 'F' emit ;   \ cursor previous line

\ Move the cursor to a specific row and column.
\ 
\ CSI row ; col H
\
\ ansi-CUP.HOME is redundant with ansi-cursor-HOME.
 
: ansi-cup-home          
   27 emit 'H' emit ;

: ansi-cup ( row col -- )
   swap 27 emit . ';' emit 'H' emit ;

\ erase screen current to end, beginning to current, or full.

: ansi-ed-from
   ansi-csi '0' emit 'J' emit ;

: ansi-ed-to
   ansi-csi '1' emit 'J' emit ;

: ansi-ed
   ansi-csi '2' emit 'J' emit ;

\ erase line current to end, beginning to current, or full.

: ansi-el-from
   ansi-csi '0' emit 'K' emit ;

: ansi-el-to
   ansi-csi '1' emit 'K' emit ;    

: ansi-el
   ansi-csi '2' emit 'K' emit ;

\ Select Graphics Rendition
\
\ Use these to assemble a sequence that will modify the text
\ display attributes. Think of these as a subset of Emacs
\ font faces.
\
\ CSI p...p SGR
\
\ Where:
\
\ CSI     is the introducer
\ p...p   are parameters, delimited by semicolons
\ SGR     is 'm'
\
\ For example, for bright red forground text:
\
\ `ansi-csi sgr-bright ansi-delim sgr-fg-red ansi-sgr`
\
\ Most of these use the 4 bit (0-7) ANSI color slots. Mixing
\ colors is described later.
\
\ These words expect nothing on the stack.

: ansi-sgr             \ end of command sequence
   'm' emit ;

: sgr-default
   '0' emit ;

: sgr-bright           \ bold/bright/insensified
   '1' emit ;

: sgr-no-bright
   '2' emit '2' emit ;

: sgr-underline
   '4' emit ;

: sgr-no-underline
   '2' emit '4' emit ;

: sgr-reverse          \ negative
   '7' emit ;

: sgr-no-reverse       \ positive
   '2' emit '7' emit ;

: sgr-italics
   '3' emit ;

: sgr-fg-black
   '3' emit '0' emit ;

: sgr-fg-red
   '3' emit '1' emit ;

: sgr-fg-green
   '3' emit '2' emit ;

: sgr-fg-yellow
   '3' emit '3' emit ;

: sgr-fg-blue
   '3' emit '4' emit ;

: sgr-fg-magenta
   '3' emit '5' emit ;

: sgr-fg-cyan
   '3' emit '6' emit ;

: sgr-fg-white
   '3' emit '7' emit ;

: sgr-fg-extended
   '3' emit '8' emit ;

: sgr-fg-default
   '3' emit '9' emit ;

: sgr-bg-black
   '4' emit '0' emit ;

: sgr-bg-red
   '4' emit '1' emit ;

: sgr-bg-green
   '4' emit '2' emit ;

: sgr-bg-yellow
   '4' emit '3' emit ;

: sgr-bg-blue
   '4' emit '4' emit ;

: sgr-bg-magenta
   '4' emit '5' emit ;

: sgr-bg-cyan
   '4' emit '6' emit ;

: sgr-bg-white
   '4' emit '7' emit ;

: sgr-bg-extended
   '4' emit '8' emit ;

: sgr-bg-default
   '4' emit '9' emit ;

\ if needed, 90- 99 bright foreground with no extended or
\                   default.
\           100-109 bright background with no extended or
\                   default


\ Select Graphic Rendition with color mixing
\
\ From the sgr-?b.* above, the extended (digit 8) command
\ allows for custom colors.
\
\ Extended colors (3|4)8; 2;r;g;b or 5;s
\
\ Where r, g, and b are 0-255
\ and   s           is an index into a color table
\                   (88 or 256 entries)
\
\ I don't expect to implement these, but if I did the rgb
\ words would be:
\
\ : sgr-fg-rgb ( r g b -- )
\ : sgr-bg-rgb

\ Frequently used helpers
\
\ Things I expect to use often enough to give friendlier
\ names:
\ 
\ Change 'ink' colors and provide string prints helpers for
\ command line output. More colors and faces can be added
\ as needed.

\ Set to a specific color or back to default.
\ All of these ( -- )

: text-red
   ansi-csi sgr-fg-red ansi-sgr ;

: text-green
   ansi-csi sgr-fg-green ansi-sgr ;

: text-blue
   ansi-csi sgr-fg-blue ansi-sgr ;

: text-cyan
   ansi-csi sgr-fg-cyan ansi-sgr ;

: text-default
   ansi-csi sgr-fg-default ansi-sgr ;
       
\ These render a string in color and then return to default
\ settings.
\ These ( str len -- )

: type-red
   text-red type text-default ;

: type-green
   text-green type text-default ;

: type-blue
   text-blue type text-default ;

: type-cyan
   text-cyan type text-default ;

\ Confirmation tests.

\ cr
\ cr s" am i red?" type-red
\ cr s" am i green?" type-green
\ cr s" am i blue?" type-blue
\ cr s" am i cyan?" type-cyan
\ cr

\ end of TxbAnsiTerminal.fs
