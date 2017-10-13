//
//  pay_oc.m
//  Unity-iPhone
//
//  Created by jue on 2017/3/21.
//
//

#import <Foundation/Foundation.h>
#import "UnityAppController.h"
//#include "UnityViewControllerBaseiOS.h"

#if defined (__cplusplus)
extern "C"
{
#endif
#pragma mark   ==============点击订单模拟支付行为==============
    char* MakeStringCopy (const char* string)
    {
        
        if (string == NULL)
            return NULL;
        
        char* res = (char*)malloc(strlen(string) + 1);
        
        strcpy(res, string);
        
        return res;
        
    }

    
    NSString * _CreateNSString (char* string)
    {
        
//        if
//            
//            (string)
//
            return
            [[NSString alloc] initWithUTF8String:string];
        
//        else
//            
//            return
//            
//            [[NSString alloc] initWithUTF8String: ""];
    }

    void iospay(int price, char* string)
    {
        printf("->%s",string);
//        NSString *playerid =_CreateNSString(string);//@"100金币" ;//_CreateNSString(string);
//        printf("->%s",playerid);
        [GetAppController() playIos:price playerid:_CreateNSString(string)];
    }
    
    
    
#if defined (__cplusplus)
}
#endif
