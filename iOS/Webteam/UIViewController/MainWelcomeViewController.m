//
//  WelcomeViewController.m
//  Webteam
//
//  Created by Maximilien Rietsch on 11/03/2015.
//  Copyright (c) 2015 Maximilien Rietsch. All rights reserved.
//

#import "MainWelcomeViewController.h"

@interface MainWelcomeViewController ()

@property (nonatomic, strong) MenuWelcomeViewController *menuController;

@end

@implementation MainWelcomeViewController

- (void)viewDidLoad {
    [super viewDidLoad];
    
    
    self.menuController = [[MenuWelcomeViewController alloc] initWithNibName:nil bundle:nil];
    sideMenuController = [[TWTSideMenuViewController alloc] initWithMenuViewController:self.menuController mainViewController:self.presentedViewController];
    self.sideMenuViewController.delegate = self;
    self.navigationItem.hidesBackButton = YES;
    
    
    self.title = @"Webteam";
    
    //Setting left button to show menu
    UIImage *temp = [[UIImage imageNamed:@"Pictures/menu-icon.png"] imageWithRenderingMode: UIImageRenderingModeAlwaysOriginal];
    UIBarButtonItem *openItem = [[UIBarButtonItem alloc] initWithImage:temp style:UIBarButtonItemStylePlain target:self action:@selector(openButtonPressed)];
    self.navigationItem.leftBarButtonItem = openItem;
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

- (void)openButtonPressed
{
    NSLog(@"pressed");
    [self.sideMenuViewController openMenuAnimated:YES completion:nil];
}

@end
