describe('My First PACTest', function (){
    it('Ouvrir url PAC', function (){
        cy.visit('https://localhost:44348/Identity/Account/Login?ReturnUrl=%2F')

    })
})
