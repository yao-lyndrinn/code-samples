#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <stdbool.h>
#include <time.h>

int pow_2(int pow){
    return 1<<pow;
}

int ones(int num_ones){
    return (1<<num_ones)-1;
}

int lower_N_bits(int N, int input){
    return (input&ones(N));
}

int get_rid_of_lower_N_bits(int N, int input){
    return (input>>N)<<N;
}

int preserve_and_replace(int N, int input, int replace){
    return (input&ones(N)) | ((replace)<<N);
}

int log2(int n) {
    int r=0;
    while (n>>=1) r++;
    return r;
}

struct entry{
    bool valid;
    unsigned int tag;
    clock_t usedAgo;
    char data;
};

struct memArray{
    char storedByte;
};


int main(int argc, char* argv[]) {
    FILE* fr;
    unsigned int cacheSizekB;
    unsigned int associativity;
    unsigned int blocksizeBytes;
    sscanf(argv[2], "%d", &cacheSizekB);
    sscanf(argv[3], "%d", &associativity);
    sscanf(argv[4], "%d", &blocksizeBytes);

    unsigned int sets = cacheSizekB * pow_2(10) / blocksizeBytes / associativity;

    struct entry** cache = (struct entry**) calloc(sizeof(struct entry*), sets);
    for (int i = 0; i < sets; i++){
        cache[i] = (struct entry*) calloc(sizeof(struct entry), associativity);
    }

    struct memArray* memory = (struct memArray*) calloc(sizeof(struct memArray), 16 * pow_2(20));

    fr = fopen(argv[1], "r");

    char instruction[5];
    unsigned int address;
    unsigned int sizeAccess;
    int blockOffsetBits = log2(blocksizeBytes);
    int indexBits = log2(sets);
    int tagBits = 24 - indexBits - blockOffsetBits;

    while (fscanf(fr, "%s", instruction) != EOF){
        fscanf(fr, "%x", &address);
        unsigned int blockOffset = lower_N_bits(blockOffsetBits, address);
        unsigned int index = lower_N_bits(indexBits, address>>blockOffsetBits);
        unsigned int currentTag = lower_N_bits(tagBits, address >> (blockOffsetBits+indexBits));
        fscanf(fr, "%d", &sizeAccess);

        bool store;
        if(strcmp(instruction, "store") == 0){
            store = true;
            //printf("store: true\n");
        }
        else{
            store = false;
            //printf("store: false\n");
        }
        //printf("address: 0x%x\n", address);
        //printf("size of access: %d \n", sizeAccess);

        //hit or miss?
        int updatedWay;
        bool hit = 0;
        for (int i = 0; i < associativity; i++){
            if (cache[index][i].valid == 1){//skip if invalid
                if(cache[index][i].tag == currentTag){
                    hit = 1;
                    updatedWay = i;
                    break;
                }
            }
        }

        //store
        if(strcmp(instruction, "store") == 0){
            unsigned char valuedata[sizeAccess];
            for (int a = 0; a < sizeAccess; a++){
                fscanf(fr, "%02hhx", &valuedata[a]);
            }
            //printf("value to store: ");
            //for(int i = 0; i < sizeAccess; i++){
            //    printf("%02hhx", valuedata[i]);
            //}
            for(int i = 0; i < sizeAccess; i++){            
                memory[address + i].storedByte = valuedata[i];
            }
            if(hit){
                cache[index][updatedWay].usedAgo = clock();
            }
        //}
        //printf("\n");
        }



        //load
        if(!store){
            //load miss update cache
            if(!hit){
                updatedWay = 0;
                bool fullSet;
                for (int i = 0; i < associativity; i++){
                    fullSet = false;
                    if (cache[index][i].valid == 0){//if invalid, update that one
                        cache[index][i].valid = 1;
                        //printf("tag in cache: %d\n", cache[index][i].tag);
                        cache[index][i].tag = currentTag;
                        cache[index][i].usedAgo = clock();
                        updatedWay = i;
                        break;
                    }
                    fullSet = true;
                }
                if (fullSet){
                    clock_t lowestUsedAgo = cache[index][0].usedAgo;
                    for (int i = 0; i < associativity; i++){
                        if (cache[index][i].usedAgo < lowestUsedAgo){//find which is LRU
                            lowestUsedAgo = cache[index][i].usedAgo;
                            updatedWay = i;
                        }
                    }
                    cache[index][updatedWay].usedAgo = clock();
                    //printf("tag in cache: %d\n", cache[index][updatedWay].tag);
                    cache[index][updatedWay].tag = currentTag;
                }
            }
            else{
                //load hit
                //printf("tag in cache: %d\n", cache[index][updatedWay].tag);
                cache[index][updatedWay].usedAgo = clock();
            }
        }

        if(store){
            if(hit){
                printf("store 0x%x hit\n", address);
            }
            else{
                printf("store 0x%x miss\n", address);
            }
        }
        else{
            if(hit){
                printf("load 0x%x hit ", address);
                for(int i = 0; i < sizeAccess; i++){
                    printf("%02hhx", memory[address + i].storedByte);
                }
                printf("\n");
                
            }
            else{
                printf("load 0x%x miss ", address);
                for(int i = 0; i < sizeAccess; i++){
                    printf("%02hhx", memory[address + i].storedByte);
                }
                printf("\n");
            }
        }
        //printf("set: %d tag: %d\n", index, currentTag);
        //printf("tag in cache: %d\n\n", cache[index][updatedWay].tag);
    }
    for (int i = 0; i < sets; i++){
        free(cache[i]);
    }
    free(cache);
    return EXIT_SUCCESS;
}
