Feature: Character and Battle
  As an API client
  I want to manage characters and battles
  So that I can create, consult and simulate fights

  # -------------------------------
  # EMPTY LIST IN BEGGING
  # -------------------------------

  Scenario: 1.0 - Get list when no characters exist
    When I GET "api/characters"
    Then the response status should be 200
    And the response should contain an empty character list

  # -------------------------------
  # CREATE CHARACTER
  # -------------------------------
  Scenario: 2.1 - Create a valid character successfully
    Given I have a character body:
      | name | job     |
      | John | Warrior |
    When I POST to "api/characters"
    Then the response status should be 200
    And the response should contain a character id

  Scenario: 2.2 - Try to create a character with invalid name
    Given I have a character body:
      | name | job  |
      | Jo   | Mage |
    When I POST to "api/characters"
    Then the response status should be 400

  Scenario: 2.3 - Try to create a character with missing fields
    Given I have a character body:
      | name | job |
      |      |     |
    When I POST to "api/characters"
    Then the response status should be 400

  # -------------------------------
  # GET CHARACTERS
  # -------------------------------
  Scenario: 3.1 - Get character list
    When I GET "api/characters"
    Then the response status should be 200
    And the response should contain a list of characters

  Scenario: 3.2 - Get character by id
    Given I have created a character:
      | name | job     |
      | John | Warrior |
    When I GET "api/characters/{id}"
    Then the response status should be 200
    And the character name should be "John"

  Scenario: 3.3 - Get character by id but it does not exist
    When I GET "api/characters/99999"
    Then the response status should be 404

  # -------------------------------
  # BATTLE
  # -------------------------------
  Scenario: 4.1 - Execute a battle between two characters
    Given I have created a character:
      | name | job     |
      | John | Warrior |
    And I have created a character:
      | name | job  |
      | Anna | Mage |
    And I have a battle request using these two characters
    When I POST to "api/battles"
    Then the response status should be 200
    And the battle log should not be empty

  Scenario: 4.2 - Try to battle when one character does not exist
    Given I have created a character:
      | name | job     |
      | John | Warrior |
    And I have a battle request using character 2461B41B-CB99-48AE-9670-484F727DB80F and nonexistent character 859A0FE1-2D65-4828-9E56-E05AC196A283
    When I POST to "api/battles"
    Then the response status should be 404

  Scenario: 4.3 - Try to battle with the same character
    Given I have created a character:
      | name | job     |
      | John | Warrior |
    And I have a battle request using character 1 and the same character 1
    When I POST to "api/battles"
    Then the response status should be 400

  Scenario: 4.4 - Try to battle without sending body
    When I POST to "api/battles"
    Then the response status should be 400

  # -------------------------------
  # EDGE CASES
  # -------------------------------
  Scenario: 5.1 - Create two characters with the same name
    Given I have a character body:
      | name | job     |
      | John | Warrior |
    When I POST to "api/characters"
    Then the response status should be 200

    Given I have a character body:
      | name | job     |
      | John | Mage    |
    When I POST to "api/characters"
    Then the response status should be 200


