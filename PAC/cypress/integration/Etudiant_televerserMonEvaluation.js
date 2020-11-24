describe('login Etudiant', function (){
    it('Televerser Mon Evaluation', function (){
        cy.visit('https://localhost:44348/Identity/Account/Login?ReturnUrl=%2F')
        cy.get('#Input_Email').type('1234567@etu.cegepjonquiere.ca')
        cy.get('#Input_Password').type('Admin12345')
        cy.get('#connexion').click()
        cy.contains('Ma rencontre').click()
        cy.get('#evaluation').click()
       // cy.get('#boxbox > form:nth-child(1) > button:nth-child(12)').click()

    })
})