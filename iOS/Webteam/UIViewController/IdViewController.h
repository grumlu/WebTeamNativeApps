//
//  IdViewController.h
//  Webteam
//
//  Created by Maximilien Rietsch on 08/03/2015.
//  Copyright (c) 2015 Maximilien Rietsch. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "NXOAuth2.h"
#import "UserDataUtilities.h"

@interface IdViewController : UIViewController <UIWebViewDelegate, UIAlertViewDelegate> {
    UIActivityIndicatorView *spinner;
    UIView *newView;
    UserDataUtilities *userData;
}

@property (weak, nonatomic) IBOutlet UIWebView *loginWebView;

@end
