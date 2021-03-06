#include <stdlib.h>
#include <string.h>
#include <stdio.h>
#include <assert.h>
#include "mothra-jni-egress.h"
#include "mothra-jni-ingress.h"

JNIEXPORT void JNICALL Java_net_p2p_mothra_Start (JNIEnv *jenv, jclass jcls, jobjectArray jargs){
    int length = (*jenv)->GetArrayLength(jenv, jargs);
    char **args = (char **) malloc(length * sizeof(char *));
    if(args){
        for (int i=0; i<length; i++) {
            jstring jarg = (jstring) ((*jenv)->GetObjectArrayElement(jenv, jargs, i));
            const char *arg = (*jenv)->GetStringUTFChars(jenv, jarg, 0);
            args[i] = (char*) malloc(strlen(arg)*sizeof(char*));
            strcpy(args[i],arg);
            (*jenv)->ReleaseStringUTFChars(jenv, jarg, (const char *)arg);
        }
    }
    else{
        return;
    }
    libp2p_start(args, length);
    for (int i=0; i<length; i++) {
        free(args[i]);
    }
    free(args);
}

JNIEXPORT void JNICALL Java_net_p2p_mothra_SendGossip (JNIEnv *jenv, jclass jcls, jbyteArray jtopic, jbyteArray jdata){
    int data_length = (*jenv)->GetArrayLength(jenv, jdata);
    int topic_length = (*jenv)->GetArrayLength(jenv, jtopic);
    jbyte *topic = (jbyte *) 0 ;
    jbyte *data = (jbyte *) 0 ;
    jboolean isCopy = JNI_TRUE;
    if (jtopic) {
        topic = (*jenv)->GetByteArrayElements(jenv,jtopic,&isCopy);
        if (!topic) return ;
    }
    if (jdata) {
        data = (*jenv)->GetByteArrayElements(jenv,jdata,&isCopy);
        if (!data) return ;
    }
    libp2p_send_gossip(topic,topic_length,data,data_length);
    if (topic) (*jenv)->ReleaseByteArrayElements(jenv, jtopic, topic, 0);
    if (data) (*jenv)->ReleaseByteArrayElements(jenv, jdata, data, 0);
}

JNIEXPORT void JNICALL Java_net_p2p_mothra_SendRPC (JNIEnv *jenv, jclass jcls, jbyteArray jmethod, jint jreq_resp, jbyteArray jpeer, jbyteArray jdata){
    int data_length = (*jenv)->GetArrayLength(jenv, jdata);
    int method_length = (*jenv)->GetArrayLength(jenv, jmethod);
    int peer_length = (*jenv)->GetArrayLength(jenv, jpeer);
    jbyte *data = (jbyte *) 0 ;
    jbyte *method = (jbyte *) 0 ;
    jbyte *peer = (jbyte *) 0 ;
    jboolean isCopy = JNI_TRUE;
    if (jdata) {
        data = (*jenv)->GetByteArrayElements(jenv,jdata,&isCopy);
        if (!data) return ;
    }
    if (jpeer) {
        peer = (*jenv)->GetByteArrayElements(jenv,jpeer,&isCopy);
        if (!peer) return ;
    }
    if (jmethod) {
        method = (*jenv)->GetByteArrayElements(jenv,jmethod,&isCopy);
        if (!method) return ;
    }
    if (jreq_resp == 0){
        libp2p_send_rpc_request(method,method_length,peer,peer_length,data,data_length);
    } else if (jreq_resp == 1){
        libp2p_send_rpc_response(method,method_length,peer,peer_length,data,data_length);
    }
    if (data) (*jenv)->ReleaseByteArrayElements(jenv, jdata, data, 0);
    if (peer) (*jenv)->ReleaseByteArrayElements(jenv, jpeer, peer, 0);
    if (method) (*jenv)->ReleaseByteArrayElements(jenv, jmethod, method, 0);
}