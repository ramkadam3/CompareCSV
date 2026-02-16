Feature: CSV Data Comparison

  Scenario: Compare two CSV files for mismatches
    Given I have an expected CSV file "TestData/expected.csv"
    And I have an actual CSV file "TestData/actual.csv"
    When I compare the files using primary key fields "ID,Name"
    Then I should see the comparison results in the output directory "Output"