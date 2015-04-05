//
//  ViewController.m
//  Webteam
//
//  Created by Maximilien Rietsch on 08/03/2015.
//  Copyright (c) 2015 Maximilien Rietsch. All rights reserved.
//

#import "HomeViewController.h"
#import "NXOAuth2.h"

@interface HomeViewController ()

@end

@implementation HomeViewController

- (void)viewDidLoad {
    [super viewDidLoad];
    
    //---- setData ----
    screeWidth = [UIScreen mainScreen].bounds.size.width;
    screenHeight = [UIScreen mainScreen].bounds.size.height;
    CGFloat nav = self.navigationController.navigationBar.bounds.size.height;
    nav += [UIApplication sharedApplication].statusBarFrame.size.height;
    data = [[UserDataUtilities alloc] init];
    
    //---- Add Background ----
    self.background = [[Background alloc] setBackgroundWithScreenSize:CGSizeMake(screeWidth, screenHeight) andNavigationBarHeight:nav];
    [self.view insertSubview:self.background atIndex:0];
}

- (void)viewDidAppear:(BOOL)animated
{
    //---- Checking account ----
    hasNXOAuth2Acc = [self hasNXOAuth2Account];
    hasUserData = [data hasDataInIdentifier:@"Webteam"];
}

- (void)didReceiveMemoryWarning {
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

- (void)viewWillTransitionToSize:(CGSize)size withTransitionCoordinator:(id<UIViewControllerTransitionCoordinator>)coordinator {
    
    [super viewWillTransitionToSize:size withTransitionCoordinator:coordinator];
    [coordinator animateAlongsideTransition:
                    ^(id<UIViewControllerTransitionCoordinatorContext> context) {
         
        CGFloat nav = self.navigationController.navigationBar.bounds.size.height;
        CGFloat statusBar = [UIApplication sharedApplication].statusBarFrame.size.height;
                        
        BOOL portrait = size.height > size.width;
        if (portrait) {
            [self.background removeFromSuperview];
            self.background = [[Background alloc] setBackgroundWithScreenSize:CGSizeMake(screeWidth, screenHeight) andNavigationBarHeight:nav + statusBar];
            [self.view insertSubview:self.background atIndex:0];
        } else {
            [self.background removeFromSuperview];
            self.background = [[Background alloc] setBackgroundWithScreenSize:CGSizeMake(screenHeight, screeWidth) andNavigationBarHeight:nav];
            [self.view insertSubview:self.background atIndex:0];
        }
     }
                    completion:^(id<UIViewControllerTransitionCoordinatorContext> context) {
     }];
    
}

#pragma mark - Specific methods

- (BOOL)hasNXOAuth2Account
{
    NXOAuth2Account *hasAccount;
    
    for (NXOAuth2Account *account in [[NXOAuth2AccountStore sharedStore] accounts]) {
        hasAccount = account;
    };
    NSLog(@"token : %@",hasAccount.accessToken.accessToken);
    
    if (hasAccount) {
        return YES;
    } else {
        return NO;
    }
}

#pragma mark IBAction

- (IBAction)webteam_button:(id)sender {
    if (hasNXOAuth2Acc && hasUserData)
        [self performSegueWithIdentifier:@"webteam" sender:self];
    else
        [self performSegueWithIdentifier:@"identification" sender:self];
}
@end
