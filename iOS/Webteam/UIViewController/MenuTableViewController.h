//
//  MenuTableViewController.h
//  Webteam
//
//  Created by Maximilien Rietsch on 15/03/2015.
//  Copyright (c) 2015 Maximilien Rietsch. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "UserDataUtilities.h"

@interface MenuTableViewController : UITableViewController <UIAlertViewDelegate> {
    NSDictionary *allItems;
    NSArray *webteamSection;
    UserDataUtilities *dataUtilities;
}

@end
