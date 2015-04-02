//
//  IdViewController.m
//  Webteam
//
//  Created by Maximilien Rietsch on 08/03/2015.
//  Copyright (c) 2015 Maximilien Rietsch. All rights reserved.
//

#import "IdViewController.h"

static NSString * const ClientId = @"5_2ikdlas7da0wkg8w8g4og48csw0s0sc4s0wk080s08gcocc404";
static NSString * const ClientSecret = @"1uz497o7324kcs0g044wkowoww8cssg4ocksw84swo8gkk8k0c";
static NSString * const RedirectURL = @"https://webteam.ensea.fr/oauth/v2/done";

static NSString * const AuthorizationURL = @"https://webteam.ensea.fr/oauth/v2/auth";
static NSString * const TokenURL = @"https://webteam.ensea.fr/oauth/v2/token";


static NSString * const Scope = @"user";
static NSString * const AccountType = @"Webteam";

@interface IdViewController ()

@property (nonatomic, strong) NSURLSession *session;

@end

@implementation IdViewController

- (void)viewDidLoad {
    [super viewDidLoad];
    
    //---- Prepare spinner ----
    [self setCurrentView];
    
    //---- Loading methods ----
    userData = [[UserDataUtilities alloc] init];
    
    //---- Delegate UIWebView ----
    self.loginWebView.delegate = self;
    
    //---- Remove all accounts ----
    for (NXOAuth2Account *account in [[NXOAuth2AccountStore sharedStore] accounts]) {
        [[NXOAuth2AccountStore sharedStore] removeAccount:account];
    };
    
    //---- Setup new accounts ----
    [self setupOAuth2AccountStore];
    [self requestOAuth2Access];
}

- (void)didReceiveMemoryWarning {
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

#pragma mark - UIWebViewDelegate methods

- (void)webViewDidFinishLoad:(UIWebView *)webView
{
    //NSLog(@"webview finished load");
    
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
                                                          
                                                          NSLog(@"Success!! We have an access token");
                                                          [self requestOAuth2ProtectedDetails];
                                                          
                                                      } else {
                                                          //account removed, we lost access
                                                          [self errorProcess];
                                                      }
                                                  }];
    
    [[NSNotificationCenter defaultCenter] addObserverForName:NXOAuth2AccountStoreDidFailToRequestAccessNotification
                                                      object:[NXOAuth2AccountStore sharedStore]
                                                       queue:nil
                                                  usingBlock:^(NSNotification *aNotification){
                                                      NSError *error = [aNotification.userInfo objectForKey:NXOAuth2AccountStoreErrorKey];
                                                      NSLog(@"Error!! %@", error.localizedDescription);
                                                      [self errorProcess];
                                                  }];
}

#pragma mark - Request OAuth2

-(void)requestOAuth2Access
{
    [[NXOAuth2AccountStore sharedStore] requestAccessToAccountWithType:AccountType
                                   withPreparedAuthorizationURLHandler:^(NSURL *preparedURL){
                                       [self.loginWebView loadRequest:[NSURLRequest requestWithURL:preparedURL]];
                                   }];
}

- (void)requestOAuth2ProtectedDetails
{
    NXOAuth2AccountStore *store = [NXOAuth2AccountStore sharedStore];
    NSArray *accounts = [store accountsWithAccountType:AccountType];
    
    [NXOAuth2Request performMethod:@"GET"
                        onResource:[NSURL URLWithString:@"https://webteam.ensea.fr/api/user"]
                   usingParameters:nil
                       withAccount:accounts[0]
               sendProgressHandler:^(unsigned long long bytesSend, unsigned long long bytesTotal) {
                   // e.g., update a progress indicator
               }
                   responseHandler:^(NSURLResponse *response, NSData *responseData, NSError *error){
                       // Process the response
                       if (responseData)
                       {
                           //---- Saving Data ----
                           [userData saveUserDataWithDictionary:
                            [userData serializeDataWithData:responseData]
                                                  andIdentifier:AccountType];
                           
                           //---- Success, dowload picture ----
                           [self requestPictureAccount];
                       }
                       if (error) {
                           NSLog(@"error : %@", error.localizedDescription);
                       }
                   }];
    
}

- (void)requestPictureAccount
{
    NXOAuth2AccountStore *store = [NXOAuth2AccountStore sharedStore];
    NSArray *accounts = [store accountsWithAccountType:AccountType];
    NSString *identifier = [userData getSpecificUserDataWithIdentifier:AccountType atKey:@"photo"];
    identifier = [identifier substringFromIndex:([identifier rangeOfString:@"photo"].location)+6];
    identifier = [identifier substringToIndex:[identifier rangeOfString:@"."].location];
    NSString *stringURL = [NSString stringWithFormat:
                           @"https://webteam.ensea.fr/api/users/%@/photo",
                           identifier];
    
    [NXOAuth2Request performMethod:@"GET"
                        onResource:[NSURL URLWithString:stringURL]
                   usingParameters:nil
                       withAccount:accounts[0]
               sendProgressHandler:^(unsigned long long bytesSend, unsigned long long bytesTotal) {
                   
               }
                   responseHandler:^(NSURLResponse *response, NSData *responseData, NSError *error){
                       
                       if (responseData) {
                           NSArray *paths = NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES);
                           NSString *documentsPath = [paths objectAtIndex:0];
                           NSString *imagePath = [documentsPath stringByAppendingPathComponent:[NSString stringWithFormat:@"%@.jpg", identifier]];
                           
                           //---- Write image data to user's folder ----
                           [[NSFileManager defaultManager] createFileAtPath:imagePath
                                                                   contents:responseData
                                                                 attributes:nil];
                           
                           //---- Store path in NSUserDefaults ----
                           [userData saveSpecificUserData:[NSString stringWithFormat:@"%@.jpg", identifier] withIdentifier:AccountType atKey:@"picture"];
                           
                           //---- Access to next page ----
                           [self performSegueWithIdentifier: @"access" sender:self];
                       }
                       if (error) {
                           NSLog(@"error : %@", error.localizedDescription);
                       }
                   }];

}

#pragma mark - Personnal methods

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

- (void)errorProcess
{
    //---- Remove accounts ----
    for (NXOAuth2Account *account in [[NXOAuth2AccountStore sharedStore] accounts]) {
        [[NXOAuth2AccountStore sharedStore] removeAccount:account];
    };
    
    //---- dissmiss spinner ----
    [spinner stopAnimating];
    [newView removeFromSuperview];
    
    //---- Show alert view ----
    UIAlertView *warning = [[UIAlertView alloc] initWithTitle:@"Account" message:@"We are not able to get access to your account.\n Would you like to try again ?" delegate:self cancelButtonTitle:@"No" otherButtonTitles:@"Yes", nil];
    [warning show];
}

- (void)alertView:(UIAlertView *)alertView clickedButtonAtIndex:(NSInteger)buttonIndex
{
    if (buttonIndex == 1)
    {
        //---- OK is pressed -----
        [self setupOAuth2AccountStore];
        [self requestOAuth2Access];
    } else {
        [self.navigationController popViewControllerAnimated:YES];
    }
}

@end
