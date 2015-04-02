//
//  MenuTableViewController.m
//  Webteam
//
//  Created by Maximilien Rietsch on 15/03/2015.
//  Copyright (c) 2015 Maximilien Rietsch. All rights reserved.
//

#import "MenuTableViewController.h"
#import "BackgroundColor.h"
#import "NXOAuth2.h"
#import "SWRevealViewController.h"
#import "UIColorCustomClass.h"

static NSString * const AccountType = @"Webteam";

@interface MenuTableViewController ()

@end

@implementation MenuTableViewController

- (void)viewDidLoad {
    [super viewDidLoad];
    
    //---- Set Background ----
    self.view.backgroundColor = [UIColor customDarkGrayColor];
    
    //---- Set tableView ----
    allItems = @{ @"Webteam" : @[@"Profil", @"Home", @"Cell1", @"Cell2", @"Cell3"],
                  @"Session" : @[@"Close"]};
    webteamSection = [allItems allKeys];
    
    //---- Preset Data ----
    dataUtilities = [[UserDataUtilities alloc] init];
}

- (void)didReceiveMemoryWarning {
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

#pragma mark - UITableViewDelegate

- (NSInteger)numberOfSectionsInTableView:(UITableView *)tableView {
    // Return the number of sections.
    return webteamSection.count;
}

- (NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section {
    // Return the number of rows in the section.
    NSString *sectionTitle = [webteamSection objectAtIndex:section];
    NSArray *sectionItems = [allItems objectForKey:sectionTitle];
    return sectionItems.count;
}

- (BOOL)tableView:(UITableView *)tableView shouldHighlightRowAtIndexPath:(NSIndexPath *)indexPath {
    return YES;
}

- (void)tableView:(UITableView *)tableView didHighlightRowAtIndexPath:(NSIndexPath *)indexPath {
    UITableViewCell *cell = [tableView cellForRowAtIndexPath:indexPath];
    cell.contentView.backgroundColor = [UIColor customDarkGrayColor];
    cell.backgroundColor = [UIColor customDarkGrayColor];
}

- (void)tableView:(UITableView *)tableView didUnhighlightRowAtIndexPath:(NSIndexPath *)indexPath {
    UITableViewCell *cell = [tableView cellForRowAtIndexPath:indexPath];
    cell.contentView.backgroundColor = [UIColor customGrayColor];
    cell.backgroundColor = [UIColor customGrayColor];
}

- (UITableViewCell *)tableView:(UITableView *)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath {
    
    // Configure the cell...
    NSString *sectionTitle = [webteamSection objectAtIndex:indexPath.section];
    NSArray *sectionItems = [allItems objectForKey:sectionTitle];
    NSString *item = [sectionItems objectAtIndex:indexPath.row];
    UITableViewCell *cell = [tableView dequeueReusableCellWithIdentifier:item forIndexPath:indexPath];
    
    cell.textLabel.text = item;
    cell.textLabel.textColor = [UIColor customFusiaColor];
    CAGradientLayer *shineLayer = [CAGradientLayer layer];
    shineLayer.frame = cell.layer.bounds;
    shineLayer.colors = [NSArray arrayWithObjects:
                         (id)[UIColor colorWithWhite:1.0f alpha:0.4f].CGColor,
                         (id)[UIColor colorWithWhite:1.0f alpha:0.2f].CGColor,
                         (id)[UIColor colorWithWhite:0.75f alpha:0.2f].CGColor,
                         (id)[UIColor colorWithWhite:0.4f alpha:0.2f].CGColor,
                         (id)[UIColor colorWithWhite:1.0f alpha:0.4f].CGColor,
                         nil];
    shineLayer.locations = [NSArray arrayWithObjects:
                            [NSNumber numberWithFloat:0.0f],
                            [NSNumber numberWithFloat:0.4f],
                            [NSNumber numberWithFloat:0.5f],
                            [NSNumber numberWithFloat:0.8f],
                            [NSNumber numberWithFloat:1.0f],
                            nil];
    [cell.layer addSublayer:shineLayer];
    
    [cell setBackgroundColor:[UIColor customGrayColor]];
    [tableView setSectionIndexColor:[UIColor customFusiaColor]];
    
    //---- Configure specific cells ----
    if ([item isEqualToString:@"Profil"]) {
        UIImageView *profilImage = [[UIImageView alloc] initWithFrame:CGRectMake(5, 5, 70, 70)];
        NSArray *paths = NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES);
        NSString *documentsPath = [paths objectAtIndex:0];
        NSString *imagePath = [documentsPath stringByAppendingPathComponent:[dataUtilities getSpecificUserDataWithIdentifier:AccountType atKey:@"picture"]];
        if ([[NSFileManager defaultManager] fileExistsAtPath:imagePath])
        {
            //File exists
            NSData *file1 = [[NSData alloc] initWithContentsOfFile:imagePath];
            if (file1)
            {
                profilImage.image = [UIImage imageWithData:file1];
            }
        }
        else
        {
            NSLog(@"File does not exist");
            profilImage.image = [UIImage imageNamed:@"Pictures/anonymous.jpg"];
        }
        profilImage.contentMode = UIViewContentModeScaleAspectFit;
        profilImage.layer.cornerRadius = profilImage.frame.size.width / 2;
        profilImage.clipsToBounds = YES;
        profilImage.layer.borderWidth = 3.0f;
        profilImage.layer.borderColor = [UIColor whiteColor].CGColor;
        [cell.contentView addSubview:profilImage];
        
        //---- Set User Name ----
        UILabel *name = [[UILabel alloc] initWithFrame:CGRectMake(90, 20, 160, 20)];
        name.text = [NSString stringWithFormat:@"%@ %@",
                     [dataUtilities getSpecificUserDataWithIdentifier:AccountType atKey:@"lastName"],
                     [dataUtilities getSpecificUserDataWithIdentifier:AccountType atKey:@"firstName"]];
        name.textColor = [UIColor customFusiaColor];
        [cell.contentView addSubview:name];
        
        //---- Set User age and birthday ----
        UILabel *age = [[UILabel alloc] initWithFrame:CGRectMake(90, 40, 160, 20)];
        NSString *date = [dataUtilities getSpecificUserDataWithIdentifier:AccountType atKey:@"date"];
        date = [date substringToIndex:[date rangeOfString:@" "].location];
        NSString *birthday = [NSString stringWithFormat:@"%@ (%@)",
                              date,
                              [dataUtilities getUserAgeWithIdentifier:AccountType]];
        age.text = birthday;
        age.textColor = [UIColor customFusiaColor];
        [cell.contentView addSubview:age];
    }
    return cell;
}

- (BOOL)tableView:(UITableView *)tableView canEditRowAtIndexPath:(NSIndexPath *)indexPath {
    return NO;
}

- (NSString *)tableView:(UITableView *)tableView titleForHeaderInSection:(NSInteger)section
{
    NSString *title = nil;
    if ([[webteamSection objectAtIndex:section] isEqualToString:@"Webteam"]) {
        title = [webteamSection objectAtIndex:section];
    }
    return title;
}

-(CGFloat)tableView:(UITableView *)tableView heightForRowAtIndexPath:(NSIndexPath *)indexPath
{
    NSString *sectionTitle = [webteamSection objectAtIndex:indexPath.section];
    NSArray *sectionItems = [allItems objectForKey:sectionTitle];
    NSString *item = [sectionItems objectAtIndex:indexPath.row];
    if ([item isEqualToString:@"Profil"]) {
        return 80.0f;
    }
    return 44.0f;
}

-(UIView *)tableView:(UITableView *)tableView viewForHeaderInSection:(NSInteger)section
{
    if (section == 0) {
        UIView *headerView = [[UIView alloc] init];
        UIImage *logoImage = [UIImage imageNamed:@"Pictures/logo.png"];
        UIImageView *logo = [[UIImageView alloc] initWithFrame:CGRectMake(0, 15, 200, 60)];
        logo.image = logoImage;
        logo.contentMode = UIViewContentModeScaleAspectFit;
        [headerView addSubview:logo];
        return headerView;
    } else {
        return nil;
    }
}

- (CGFloat)tableView:(UITableView *)tableView heightForHeaderInSection:(NSInteger)section
{
    if (section == 0)
        return 90.0;
    else
        return 25.0;
}

- (void)tableView:(UITableView *)tableView didSelectRowAtIndexPath:(NSIndexPath *)indexPath
{
    NSString *sectionTitle = [webteamSection objectAtIndex:indexPath.section];
    NSArray *sectionItems = [allItems objectForKey:sectionTitle];
    NSString *item = [sectionItems objectAtIndex:indexPath.row];
    
    if ([item isEqualToString:@"Close"])
    {
        [self.revealViewController revealToggleAnimated:YES];
        UIAlertView *close = [[UIAlertView alloc] initWithTitle:@"Logout" message:@"Are you sure you want to logout ?" delegate:self cancelButtonTitle:@"No" otherButtonTitles:@"Yes", nil];
        [close show];
    }
}

#pragma mark - UIAlertViewDelegate

- (void)alertView:(UIAlertView *)alertView clickedButtonAtIndex:(NSInteger)buttonIndex
{
    if (buttonIndex == 1)
    {
        //---- Yes is pressed ----
        //---- Remove all accounts ----
        for (NXOAuth2Account *account in [[NXOAuth2AccountStore sharedStore] accounts]) {
          [[NXOAuth2AccountStore sharedStore] removeAccount:account];
        };
        [dataUtilities removeUserDataFromIdentifier:AccountType];
        
        //---- Close View controller ----
        [self dismissViewControllerAnimated:YES completion:nil];
    }
}

@end
