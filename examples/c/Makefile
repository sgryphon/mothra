SHELL := /bin/sh

include ../../config.mk

CFLAGS:=-Wl,-rpath='$${ORIGIN}'
INCLUDES:=-I$(CBIND_DIR)

.PHONY : examples clean

c-examples:
	$(CC) example.c $(INCLUDES) -L$(OUT_DIR)/ -lmothra-ingress -lmothra $(CFLAGS) -o $(OUT_DIR)/example

clean:
	rm -rf $(OUT_DIR)/example*