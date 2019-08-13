#include <jni.h>
#ifndef _MOTHRA_JNI_INGRESS_H_
#define _MOTHRA_JNI_INGRESS_H_

#ifdef __cplusplus
extern "C" {
#endif

JNIEXPORT void JNICALL Java_mothra_Init(JNIEnv*,jclass);

void receive_gossip(char*);

void detach(JNIEnv* );

#ifdef __cplusplus
}
#endif

#endif // _MOTHRA_JNI_INGRESS_H_