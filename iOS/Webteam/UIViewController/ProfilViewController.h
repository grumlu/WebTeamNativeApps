//
//  ProfilViewController.h
//  Webteam
//
//  Created by Maximilien Rietsch on 14/03/2015.
//  Copyright (c) 2015 Maximilien Rietsch. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "UIButtonCustomClass.h"
#import "UserDataUtilities.h"

@interface ProfilViewController : UIViewController <UITextFieldDelegate,
                                                    UITextViewDelegate,
                                                    UIScrollViewDelegate> {
    UserDataUtilities *dataUtilities;
    UITextField *activeField;
    UITextView *activeTextView;
}

@property (weak, nonatomic) IBOutlet UIScrollView *scrollView;
@property (weak, nonatomic) IBOutlet UIImageView *profil_ImageView;
@property (weak, nonatomic) IBOutlet UILabel *userName_Label;
@property (weak, nonatomic) IBOutlet UITextField *userName_TextField;
@property (weak, nonatomic) IBOutlet UILabel *lastName_Label;
@property (weak, nonatomic) IBOutlet UITextField *lastName_TextField;
@property (weak, nonatomic) IBOutlet UILabel *firstName_Label;
@property (weak, nonatomic) IBOutlet UITextField *firstName_TextField;
@property (weak, nonatomic) IBOutlet UILabel *promo_Label;
@property (weak, nonatomic) IBOutlet UITextField *promo_TextField;
@property (weak, nonatomic) IBOutlet UILabel *birthday_Label;
@property (weak, nonatomic) IBOutlet UITextField *birthday_TextField;
@property (weak, nonatomic) IBOutlet UILabel *address_Label;
@property (weak, nonatomic) IBOutlet UITextView *address_TextView;
@property (weak, nonatomic) IBOutlet UILabel *phone_Label;
@property (weak, nonatomic) IBOutlet UITextField *phone_TextField;
@property (weak, nonatomic) IBOutlet UIButtonCustomClass *modification_Button;

- (IBAction)applyModification:(id)sender;

@end
