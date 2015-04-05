//
//  ProfilViewController.m
//  Webteam
//
//  Created by Maximilien Rietsch on 14/03/2015.
//  Copyright (c) 2015 Maximilien Rietsch. All rights reserved.
//

#import "ProfilViewController.h"
#import "SWRevealViewController.h"
#import "GradientColor.h"
#import "UIColorCustomClass.h"

static NSString * const AccountType = @"Webteam";

@interface ProfilViewController ()

@end

@implementation ProfilViewController

- (void)viewDidLoad {
    [super viewDidLoad];
    
    //---- set Title ----
    self.navigationItem.title = @"Profil";
    
    //---- init Data ----
    dataUtilities = [[UserDataUtilities alloc] init];
    
    //---- delegate UI. ----
    self.scrollView.delegate = self;
    self.userName_TextField.delegate = self;
    self.firstName_TextField.delegate = self;
    self.lastName_TextField.delegate = self;
    self.promo_TextField.delegate = self;
    self.birthday_TextField.delegate = self;
    self.address_TextView.delegate = self;
    self.phone_TextField.delegate = self;
    
    //---- set UITapGestureRecognizer to dismissKeyboard ----
    UITapGestureRecognizer *tap = [[UITapGestureRecognizer alloc]
                                   initWithTarget:self
                                   action:@selector(dismissKeyboard)];
    [self.view addGestureRecognizer:tap];
    
    //---- setBackground ----
    CAGradientLayer *bgLayer = [GradientColor pinkGradient];
    bgLayer.frame = self.view.bounds;
    [self.view.layer insertSublayer:bgLayer atIndex:0];
    
    //---- set Sidebar button ----
    SWRevealViewController *revealViewController = self.revealViewController;
    if ( revealViewController )
    {
        UIBarButtonItem *openItem = [[UIBarButtonItem alloc] initWithImage:[UIImage imageNamed:@"menu-icon"] style:UIBarButtonItemStylePlain target:self action:@selector(revealToggle:)];
        self.navigationItem.leftBarButtonItem = openItem;
        [openItem setTarget: self.revealViewController];
        [self.view addGestureRecognizer:self.revealViewController.panGestureRecognizer];
    }
    
    //---- Custom function ----
    [self setTextFieldDesign];
    [self setPicture];
    [self setTextFieldValues];
    [self registerForKeyboardNotifications];
}

#pragma mark - IBAction

- (IBAction)applyModification:(id)sender {
    // Send data later
}

- (void)dismissKeyboard {
    if (activeField) {
        [activeField resignFirstResponder];
    }
    if (activeTextView) {
        [activeTextView resignFirstResponder];
    }
}

#pragma mark - UITextField/UITextView delegate

- (void)textFieldDidBeginEditing:(UITextField *)textField {
    [textField becomeFirstResponder];
    activeField = textField;
}

- (BOOL)textFieldShouldReturn:(UITextField *)textField {
    [textField resignFirstResponder];
    activeField = nil;
    return YES;
}

- (BOOL)textFieldShouldEndEditing:(UITextField *)textField {
    [textField resignFirstResponder];
    activeField = nil;
    return YES;
}

- (void)textViewDidBeginEditing:(UITextView *)textView {
    [textView becomeFirstResponder];
    activeTextView = textView;
}

- (BOOL)textViewShouldEndEditing:(UITextView *)textView {
    [textView resignFirstResponder];
    activeTextView = nil;
    return YES;
}

#pragma mark - Keyboard Notifications

- (void)registerForKeyboardNotifications
{
    [[NSNotificationCenter defaultCenter] addObserver:self
                                             selector:@selector(keyboardWasShown:)
                                                 name:UIKeyboardDidShowNotification object:nil];
    
    [[NSNotificationCenter defaultCenter] addObserver:self
                                             selector:@selector(keyboardWillBeHidden:)
                                                 name:UIKeyboardWillHideNotification object:nil];
    
}

// Called when the UIKeyboardDidShowNotification is sent.
- (void)keyboardWasShown:(NSNotification*)aNotification
{
    NSDictionary* info = [aNotification userInfo];
    CGSize kbSize = [[info objectForKey:UIKeyboardFrameBeginUserInfoKey] CGRectValue].size;
    
    UIEdgeInsets contentInsets = UIEdgeInsetsMake(0.0, 0.0, kbSize.height, 0.0);
    self.scrollView.contentInset = contentInsets;
    self.scrollView.scrollIndicatorInsets = contentInsets;
    
    // If active text field is hidden by keyboard, scroll it so it's visible
    CGRect aRect = self.view.frame;
    aRect.size.height -= kbSize.height;
    if (!CGRectContainsPoint(aRect, activeField.frame.origin) ) {
        [self.scrollView scrollRectToVisible:activeField.frame animated:YES];
    }
    if (!CGRectContainsPoint(aRect, activeTextView.frame.origin) ) {
        [self.scrollView scrollRectToVisible:activeTextView.frame animated:YES];
    }
}

// Called when the UIKeyboardWillHideNotification is sent
- (void)keyboardWillBeHidden:(NSNotification*)aNotification
{
    UIEdgeInsets contentInsets = UIEdgeInsetsZero;
    self.scrollView.contentInset = contentInsets;
    self.scrollView.scrollIndicatorInsets = contentInsets;
}

#pragma mark - setDesign

- (void)setTextFieldDesign {
    self.address_TextView.layer.borderWidth = 1;
    self.address_TextView.layer.borderColor = [[UIColor blackColor] CGColor];
    self.address_TextView.backgroundColor = [UIColor clearColor];
    //self.address_TextView.layer.cornerRadius = 6;
}

#pragma mark - setValues

- (void)setPicture {
    NSArray *paths = NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES);
    NSString *documentsPath = [paths objectAtIndex:0];
    NSString *imagePath = [documentsPath stringByAppendingPathComponent:[dataUtilities getSpecificUserDataWithIdentifier:AccountType atKey:@"picture"]];
    if ([[NSFileManager defaultManager] fileExistsAtPath:imagePath]) {
        //File exists
        NSData *file1 = [[NSData alloc] initWithContentsOfFile:imagePath];
        if (file1) {
            self.profil_ImageView.image = [UIImage imageWithData:file1];
        }
    }
    else {
        NSLog(@"File does not exist");
        self.profil_ImageView.image = [UIImage imageNamed:@"Pictures/anonymous.jpg"];
    }
    self.profil_ImageView.clipsToBounds = YES;
    self.profil_ImageView.layer.borderWidth = 1.0f;
    self.profil_ImageView.layer.borderColor = [UIColor blackColor].CGColor;
}

- (void)setTextFieldValues {
    
    self.userName_TextField.text = [dataUtilities getSpecificUserDataWithIdentifier:AccountType atKey:@"username"];
    
    self.lastName_TextField.text = [dataUtilities getSpecificUserDataWithIdentifier:AccountType atKey:@"lastName"];
    
    self.firstName_TextField.text = [dataUtilities getSpecificUserDataWithIdentifier:AccountType atKey:@"firstName"];
    
    self.promo_TextField.text = [dataUtilities getSpecificUserDataWithIdentifier:AccountType atKey:@"promo"];
    
    NSString *date = [dataUtilities getSpecificUserDataWithIdentifier:AccountType atKey:@"date"];
    date = [date substringToIndex:[date rangeOfString:@" "].location];
    self.birthday_TextField.text = date;
    
    self.address_TextView.text = [dataUtilities getSpecificUserDataWithIdentifier:AccountType atKey:@"address"];
    
    self.phone_TextField.text = [dataUtilities getSpecificUserDataWithIdentifier:AccountType atKey:@"phone"];
    
}
@end
