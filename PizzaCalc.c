#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#define PI 3.14159265358979323846

struct PizzaNode{
    char name[64];
    float diameter;
    float cost;
    float pizzaPerDollar;
    PizzaNode* next;
};

struct PizzaNode* sortNodes(struct PizzaNode* head){
    int swaps;
    //don't even bother if only one item in list
    if(head->next == NULL){
        return head;
    }
    struct PizzaNode* current = head->next;
    struct PizzaNode* previous = head;
    struct PizzaNode* headNode = head;
    do {
        swaps = 0;
        if(current->pizzaPerDollar > previous -> pizzaPerDollar){
            struct PizzaNode* temp = current;
            previous -> next = current -> next;
            current -> next = previous;
            headNode = current; //assign new head
            current = previous;
            previous = current;//adjust labels
            swaps = 1;
        } else if(current -> pizzaPerDollar == previous -> pizzaPerDollar){
            if(strcmp(current->name, previous->name) > 0){
                struct PizzaNode* temp = current;
                previous -> next = current -> next;
                current -> next = previous;
                headNode = current; //assign new head if necessary
                current = previous;
                previous = current;//adjust labels
                swaps = 1;
                }
        }
        while (current != NULL && current->next != NULL){
            //swap based on pizzaperdollar
            if (current->next->pizzaPerDollar > current->pizzaPerDollar){
                struct PizzaNode* temp = current->next;
                current->next = current->next->next;
                temp -> next = current;
                previous -> next = temp;
                swaps = 1;
            }
            //otherwise swap alphabetically
            else if (current->next->pizzaPerDollar == current->pizzaPerDollar){
                if(strcmp(current->name, current->next->name) > 0){
                    struct PizzaNode* temp = current->next;
                    current->next = current->next->next;
                    temp -> next = current;
                    previous -> next = temp;
                    swaps = 1;
                }
            }
            //printf("%s\n",current->name);
            //printf("%s\n",current->next->name);
            previous = current;
            current = current->next;
        }
        previous = headNode;
        current = headNode->next;
    } while (swaps > 0);
    return headNode;
}

struct PizzaNode* previous;
int main(int argc, char* argv[]) {
    FILE* fr;
    fr = fopen(argv[1], "r");
    struct PizzaNode* headNode = (struct PizzaNode*) malloc(sizeof(struct PizzaNode));
    struct PizzaNode* currentNode = headNode;
    //check if file is empty

    //create linked list, stop at Done or EOF
    char buffer[64];
    while (1)
    {
        if (fscanf(fr, "%s", buffer) == EOF){
            char empty[20] = "PIZZA FILE IS EMPTY";
            printf("%s", empty);
            free(headNode);
            fclose(fr);
            return EXIT_SUCCESS;
        }
        if(strcmp(buffer, "DONE") == 0){ //set equal to null and exit
            //strcpy(currentNode->name, "ZDONE");
            previous->next = NULL;
            free(currentNode);
            break;
        }
        strcpy(currentNode->name, buffer);
        fscanf(fr, "%f", &currentNode->diameter);
        fscanf(fr, "%f", &currentNode->cost);
        if(currentNode->cost == 0){
            currentNode->pizzaPerDollar = 0;
        } else{
            currentNode->pizzaPerDollar = PI * currentNode->diameter * currentNode->diameter / 4 / currentNode->cost;
        }
        currentNode->next = (struct PizzaNode*) malloc(sizeof(struct PizzaNode));
        previous=currentNode;
        currentNode = currentNode->next;
    }
    fclose(fr);

    headNode = sortNodes(headNode);
    currentNode = headNode;
    while (currentNode != NULL){
        printf("%s %f\n", currentNode->name, currentNode->pizzaPerDollar);
        currentNode = currentNode->next;
    }
    
    //printf("%s %f %f", headNode->name, headNode->diameter, headNode->cost);
    //set current equal to head and start freeing
    currentNode = headNode;
    while (currentNode != NULL){
        struct PizzaNode* tempNext = currentNode->next;
        free(currentNode);
        currentNode = tempNext;
    }
    //free(currentNode);
    //free the null node
    return EXIT_SUCCESS;
}