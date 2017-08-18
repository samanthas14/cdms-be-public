namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HabUpdateNoaaIndex : DbMigration
    {
        public override void Up()
        {
            Sql(@"
update [dbo].[MetadataProperties]
set PossibleValues = '[""01. Habitat Quantity"",""02. Injury and Mortality"",""03. Food"",""04. Riparian Condition"",""05. Peripheral and Transitional Habitats"",""06. Channel Structure and Form"",""07. Sediment Conditions"",""08. Water Quality"",""09. Water Quantity"",""10. Population Level Effects""]' 
where Name = 'NOAA Ecological Concerns'
update [dbo].[MetadataProperties]
set PossibleValues = '[""01.1 Anthropogenic Barriers"",""01.2 Natural Barriers"",""01.3 HQ-Competition"",""02.1 Predation"",""02.2 Pathogens"",""02.3 Mechanical Injury"",""02.4 Contaminated Food"",""03.1 Altered Primary Productivity"",""03.2 Food-Competition"",""03.3 Altered Prey Species Composition and Diversity"",""04.1 Riparian Condition"",""04.2 LWD Recruitment"",""05.1 Side Channel and Wetland Conditions"",""05.2 Floodplain Condition"",""05.3 Estuary Conditions"",""05.4 Nearshore Conditions"",""06.1 Bed and Channel Form"",""06.2 Instream Structural Complexity"",""07.1 Decreased Sediment Quantity"",""07.2 Increased Sediment Quantity"",""08.1 Temperature"",""08.2 Oxygen"",""08.3 Gas Saturation"",""08.4 Turbidity"",""08.5 pH"",""08.6 Salinity"",""08.7 Toxic Contaminants"",""09.1 Increased Water Quantity"",""09.2 Decreased Water Quantity"",""09.3 Altered Flow Timing"",""10.1 Reduced Genetic Adaptiveness"",""10.2 Small Population Effects"",""10.3 Demographic Changes"",""10.4 Life History Changes""]' 
where Name = 'NOAA Ecological Concerns: Sub-categories'
            ");
        }
        
        public override void Down()
        {
            Sql(@"
update [dbo].[MetadataProperties]
set PossibleValues = '[""1. Habitat Quantity"",""2. Injury and Mortality"",""3. Food"",""4. Riparian Condition"",""5. Peripheral and Transitional Habitats"",""6. Channel Structure and Form"",""7. Sediment Conditions"",""8. Water Quality"",""9. Water Quantity"",""10. Population Level Effects""]' 
where Name = 'NOAA Ecological Concerns'
update [dbo].[MetadataProperties]
set PossibleValues = '[""1.1 Anthropogenic Barriers"",""1.2 Natural Barriers"",""1.3 HQ-Competition"",""2.1 Predation"",""2.2 Pathogens"",""2.3 Mechanical Injury"",""2.4 Contaminated Food"",""3.1 Altered Primary Productivity"",""3.2 Food-Competition"",""3.3 Altered Prey Species Composition and Diversity"",""4.1 Riparian Condition"",""4.2 LWD Recruitment"",""5.1 Side Channel and Wetland Conditions"",""5.2 Floodplain Condition"",""5.3 Estuary Conditions"",""5.4 Nearshore Conditions"",""6.1 Bed and Channel Form"",""6.2 Instream Structural Complexity"",""7.1 Decreased Sediment Quantity"",""7.2 Increased Sediment Quantity"",""8.1 Temperature"",""8.2 Oxygen"",""8.3 Gas Saturation"",""8.4 Turbidity"",""8.5 pH"",""8.6 Salinity"",""8.7 Toxic Contaminants"",""9.1 Increased Water Quantity"",""9.2 Decreased Water Quantity"",""9.3 Altered Flow Timing"",""10.1 Reduced Genetic Adaptiveness"",""10.2 Small Population Effects"",""10.3 Demographic Changes"",""10.4 Life History Changes""]' 
where Name = 'NOAA Ecological Concerns: Sub-categories'
            ");
        }
    }
}
