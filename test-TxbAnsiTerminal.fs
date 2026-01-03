\ test-TxbAnsiTerminal.fs -- Test Ansi Controls -- T.Brumley

   marker TEST-TXBANSITERMINAL

   require TxbAnsiTerminal.fs

\ This is very limited. I can't get the cursor controls to
\ work from interactive gforth or pforth yet so this just
\ exercises some of the graphic rendition functions that
\ should be reviewed by eye.
\ Markers and delimiters.

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

\ These on the fly sgr changes aren't working. TODO: Why?

text-default cr
." The following two on the fly sgr attempts fail. Why? " cr
ansi-csi sgr-bright sgr-fg-red ansi-sgr ." bright red?" cr

text-default

ansi-csi sgr-bg-cyan sgr-fg-yellow ansi-sgr
." yellow on cyan?" cr

text-default

\ These all work OK.
cr
cr s" am i red?" type-red
cr s" am i green?" type-green
cr s" am i blue?" type-blue
cr s" am i cyan?" type-cyan
cr

\ end of test-TxbAnsiTerminal.fs
