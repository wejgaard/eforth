#define NEXT goto next
#define JMPW goto work
#define ADDR_DOCOLON ((void *) OP_DOCOLON)
#define ADDR_DOCREATE ((void *) OP_DOCREATE)
#define ADDR_DODOES ((void *) OP_DODOES)

enum {
  OP_DOCOLON = 0,
  OP_DOCREATE,
  OP_DODOES,
#define X(name, op, code) OP_ ## op,
  PLATFORM_OPCODE_LIST
  OPCODE_LIST
#undef X
};

static cell_t *forth_run(cell_t *init_rp) {
  if (!init_rp) {
#define X(name, op, code) \
    create(name, sizeof(name) - 1, name[0] == ';', (void *) OP_ ## op);
    PLATFORM_OPCODE_LIST
    OPCODE_LIST
#undef X
    return 0;
  }
  register cell_t *ip, *rp, *sp, tos, w;
  rp = init_rp;  ip = (cell_t *) *rp--;  sp = (cell_t *) *rp--;
  DROP;
  for (;;) {
next:
    w = *ip++;
work:
    switch (*(cell_t *) w & 0xff) {
#define X(name, op, code) case OP_ ## op: { code; } NEXT;
  PLATFORM_OPCODE_LIST
  OPCODE_LIST
#undef X
      case OP_DOCOLON: ++rp; *rp = (cell_t) ip; ip = (cell_t *) (w + sizeof(cell_t)); NEXT;
      case OP_DOCREATE: DUP; tos = w + sizeof(cell_t) * 2; NEXT;
      case OP_DODOES: DUP; tos = w + sizeof(cell_t) * 2;
                      ++rp; *rp = (cell_t) ip;
                      ip = (cell_t *) *(cell_t *) (w + sizeof(cell_t)); NEXT;
    }
  }
}
