//
//  IdViewController.m
//  Webteam
//
//  Created by Maximilien Rietsch on 08/03/2015.
//  Copyright (c) 2015 Maximilien Rietsch. All rights reserved.
//

#import "IdViewController.h"
#import "NXOAuth2.h"

static NSString * const ClientId = @"5_2ikdlas7da0wkg8w8g4og48csw0s0sc4s0wk080s08gcocc404";
static NSString * const ClientSecret = @"1uz497o7324kcs0g044wkowoww8cssg4ocksw84swo8gkk8k0c";
static NSString * const RedirectURL = @"https://webteam.ensea.fr/oauth/v2/done";

static NSString * const AuthorizationURL = @"https://webteam.ensea.fr/oauth/v2/auth";
static NSString * const TokenURL = @"https://webteam.ensea.fr/oauth/v2/token";


static NSString * const Scope = @"user";
static NSString * const AccountType = @"Webteam";
static NSString * const SuccessPagePrefix = @"Success";

@interface IdViewController ()

@end

@implementation IdViewController

- (void)viewDidLoad {
    [super viewDidLoad];
    

    [self setCurrentView];
    
    self.loginWebView.delegate = self;
    
    [self setupOAuth2AccountStore];
    [self requestOAuth2Access];
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

#pragma mark - UIWebViewDelegate methods

- (void)webViewDidFinishLoad:(UIWebView *)webView
{
    NSLog(@"webview finished load");
    
    if ([webView.request.URL.absoluteString rangeOfString:AuthorizationURL options:NSCaseInsensitiveSearch].location != NSNotFound) {
        self.loginWebView.hidden = NO;
    
    } else {
        //otherwise hide the UIWebView, we've left the authorization flow
        self.loginWebView.hidden = YES;
        
        //---- Adding ----
        [self.view addSubview:newView];
        [newView addSubview:spinner];
        [spinner startAnimating];
        
        //---- Last Request ----
        [[NXOAuth2AccountStore sharedStore] handleRedirectURL:webView.request.URL];
    }
}

#pragma mark - setup oauth2

- (void)setupOAuth2AccountStore
{

    [[NXOAuth2AccountStore sharedStore] setClientID:ClientId
                                        secret:ClientSecret
                                        scope:[NSSet setWithObjects:Scope, nil]
                                        authorizationURL:[NSURL URLWithString:AuthorizationURL]
                                        tokenURL:[NSURL URLWithString:TokenURL]
                                        redirectURL:[NSURL URLWithString:RedirectURL]
                                        keyChainGroup:@"code"
                                        forAccountType:AccountType];
    
    [[NSNotificationCenter defaultCenter] addObserverForName:NXOAuth2AccountStoreAccountsDidChangeNotification
                                                      object:[NXOAuth2AccountStore sharedStore]
                                                       queue:nil
                                                  usingBlock:^(NSNotification *aNotification){
                                                      
                                                      if (aNotification.userInfo) {
                                                          //account added, we have access
                                                          //we can now request protected data
                                                          NSLog(@"Success!! We have an access token.");
                                                          NSLog(@"%@ ---- %@",aNotification.description,aNotification.userInfo);
                                                          
                                                          [self performSegueWithIdentifier: @"access" sender:self];
                                                          
                                                      } else {
                                                          //account removed, we lost access
                                                      }
                                                  }];
    
    [[NSNotificationCenter defaultCenter] addObserverForName:NXOAuth2AccountStoreDidFailToRequestAccessNotification
                                                      object:[NXOAuth2AccountStore sharedStore]
                                                       queue:nil
                                                  usingBlock:^(NSNotification *aNotification){
                                                      
                                                      NSError *error = [aNotification.userInfo objectForKey:NXOAuth2AccountStoreErrorKey];
                                                      NSLog(@"Error!! %@", error.localizedDescription);
                                                      
                                                  }];
}

-(void)requestOAuth2Access
{
    [[NXOAuth2AccountStore sharedStore] requestAccessToAccountWithType:AccountType
                                   withPreparedAuthorizationURLHandler:^(NSURL *preparedURL){
                                       NSLog(@"%@",preparedURL.absoluteURL);
                                       //navigate to the URL returned by NXOAuth2Client
                                       [self.loginWebView loadRequest:[NSURLRequest requestWithURL:preparedURL]];
                                   }];
}

#pragma mark - Dressing UIView

-(void)setCurrentView
{
    
    //---- ActivityIndicator ----
    CGRect screenBound = [[UIScreen mainScreen] bounds];
    CGSize screenSize = screenBound.size;
    spinner = [[UIActivityIndicatorView alloc] initWithActivityIndicatorStyle:UIActivityIndicatorViewStyleWhiteLarge];
    [spinner setCenter:CGPointMake(screenSize.width/2.0, screenSize.height/2.0)];
    
    
    //---- New UIView ----
    newView = [[UIView alloc] initWithFrame:CGRectMake(
                                    0,
                                    0,
                                    [[UIScreen mainScreen] bounds].size.width,
                                    [[UIScreen mainScreen] bounds].size.height)];
    newView.backgroundColor = [UIColor blackColor];
    newView.alpha = 0.5;
    
}

@end
