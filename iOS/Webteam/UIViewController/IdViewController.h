//
//  IdViewController.h
//  Webteam
//
//  Created by Maximilien Rietsch on 08/03/2015.
//  Copyright (c) 2015 Maximilien Rietsch. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface IdViewController : UIViewController <UIWebViewDelegate> {
    UIActivityIndicatorView *spinner;
    UIView *newView;
}

@property (weak, nonatomic) IBOutlet UIWebView *loginWebView;

@end
