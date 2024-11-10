window.ShowSwal = (player) => {
    Swal.fire({
        title: player + ' won!!',
        width: 350,
        padding: '3em',
        color: '#716add',
        backdrop: `
                        rgba(0,0,123,0.4)
                      `
    })
}
window.ShowTie = () => {
    Swal.fire({
        title: 'Go home, nobody won!',
        width: 350,
        padding: '3em',
        color: '#716add',
        backdrop: `
                        rgba(0,0,123,0.4)
                      `
    })
}