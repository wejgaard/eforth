( Shared Library Handling )
1 constant RTLD_LAZY
2 constant RTLD_NOW
: dlopen ( n z -- a ) [ 0 z" dlopen" dlsym aliteral ] call2 ;
create calls
' call0 , ' call1 , ' call2 , ' call3 , ' call4 ,
' call5 , ' call6 , ' call7 , ' call8 , ' call9 ,
: sofunc ( z n a "name" -- ) swap >r swap dlsym create , r> cells calls + @ ,
                      does> dup @ swap cell+ @ execute ;
: sysfunc ( z n "name" -- ) 0 sofunc ;
: shared-library ( z "name" -- ) RTLD_NOW dlopen create , does> @ sofunc ;

( Major Syscalls )
z" close" 1 sysfunc close
z" read" 3 sysfunc read
z" write" 3 sysfunc write
z" open" 3 sysfunc open
z" creat" 2 sysfunc creat
z" exit" 1 sysfunc sysexit

( Default Pipes )
0 constant stdin
1 constant stdout
2 constant stderr

( Terminal handling )
: n. ( n -- ) base @ swap decimal <# #s #> type base ! ;
: esc   27 emit ;
: at-xy ( x y -- ) esc ." [" 1+ n. ." ;" 1+ n. ." H" ;
: page   esc ." [2J" esc ." [H" ;
: normal   esc ." [0m" ;
: fg ( n -- ) esc ." [38;5;" n. ." m" ;
: bg ( n -- ) esc ." [48;5;" n. ." m" ;
: clear-to-eol   esc ." [0K" ;
: scroll-down   esc ." D" ;
: scroll-up   esc ." M" ;
: hide   esc ." [?25l" ;
: show   esc ." [?25h" ;
: terminal-save   esc ." [?1049h" ;
: terminal-restore   esc ." [?1049l" ;

( Hookup I/O )
: stdout-write ( a n -- ) stdout -rot write drop ;
' stdout-write is type
: stdin-key ( -- n ) 0 >r stdin rp@ 1 read drop r> ;
' stdin-key is key
: posix-bye   0 sysexit ;
' posix-bye is bye
