describe('Login Enseignant', function (){
    it('Rencontre Enseignant', function (){
        cy.visit('https://localhost:44348/Identity/Account/Login?ReturnUrl=%2F')
        cy.get('#Input_Email').type('cypresstest@cegepjonquiere.ca')
        cy.get('#Input_Password').type('test123')
        cy.get('#connexion').click()
        cy.contains('Mes évaluations').click()

    })
})