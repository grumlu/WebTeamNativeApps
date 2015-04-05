//
//  LinksTableViewController.m
//  Webteam
//
//  Created by Maximilien Rietsch on 06/04/2015.
//  Copyright (c) 2015 Maximilien Rietsch. All rights reserved.
//

#import "LinksTableViewController.h"
#import "SWRevealViewController.h"
#import "GradientColor.h"
#import "UIColorCustomClass.h"

@interface LinksTableViewController ()

@end

@implementation LinksTableViewController

- (void)viewDidLoad {
    [super viewDidLoad];
    
    //---- set Title ----
    self.navigationItem.title = @"Links";
    
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
    
    //---- Set tableView ----
    allItems = @{ @"Ensea" : @[@"Ensea"],
                  @"Intranet" : @[@"Intranet"],
                  @"Moodle" : @[@"Moodle"]};
    webteamSection = [allItems allKeys];
}

#pragma mark - Table view data source

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
    
    return cell;
}

-(CGFloat)tableView:(UITableView *)tableView heightForRowAtIndexPath:(NSIndexPath *)indexPath
{
    return 60.0f;
}

@end
