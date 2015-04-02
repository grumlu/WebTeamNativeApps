//
//  WelcomeViewController.m
//  Webteam
//
//  Created by Maximilien Rietsch on 11/03/2015.
//  Copyright (c) 2015 Maximilien Rietsch. All rights reserved.
//

#import "WelcomeViewController.h"
#import "SWRevealViewController.h"

@interface WelcomeViewController ()

@end

@implementation WelcomeViewController

- (void)viewDidLoad {
    [super viewDidLoad];
    
    //---- set Title ----
    self.navigationItem.title = @"Home";
    
    //---- Set SideBar button ----
    SWRevealViewController *revealViewController = self.revealViewController;
    if ( revealViewController )
    {
        UIBarButtonItem *openItem = [[UIBarButtonItem alloc] initWithImage:[UIImage imageNamed:@"menu-icon"] style:UIBarButtonItemStylePlain target:self action:@selector(revealToggle:)];
        self.navigationItem.leftBarButtonItem = openItem;
        [openItem setTarget: self.revealViewController];
        [self.view addGestureRecognizer:self.revealViewController.panGestureRecognizer];
    }
    
}

- (void)didReceiveMemoryWarning {
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

/*
#pragma mark - Navigation

// In a storyboard-based application, you will often want to do a little preparation before navigation
- (void)prepareForSegue:(UIStoryboardSegue *)segue sender:(id)sender {
    // Get the new view controller using [segue destinationViewController].
    // Pass the selected object to the new view controller.
}
*/

@end