Feature: F1SportEvent
	The Ergast Developer API is an experimental web service which provides a historical record of motor racing data for non-commercial purposes.

@regression
Scenario Outline: Verify Driver Details from 2009 Season
Given the Formula One End Point is called with season 1990 using Get Method 

Then Verify the Drivers in the table below
	| givenName   | familyName   | nationality   | dateOfBirth   |
	| <givenName> | <familyName> | <nationality> | <dateOfBirth> |

Examples: 
	| season | driverId    | givenName | familyName  | nationality | dateOfBirth |
	| 2009   | fernando    | Fernando  | Alonso      | Spanish     | 1981-07-29  |
	| 2009   | lewis       | Lewis     | Hamilton    | English     | 1985-01-07  |
	| 2009   | alguersuari | Jaime     | Alguersuari | Spanish     | 1990-03-23  |
	| 1990   | alesi       | Jean      | Alesi       | French      | 1964-06-11  |
	| 1990   | barilla     | Paolo     | Barilla     | Italian     | 1961-04-20  |

Scenario: Verify F1 Formula one Races
		Given the Formula One End Point is called using Get Method with races.json
		Then Verify Formula One Race details