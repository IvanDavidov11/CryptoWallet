import React from 'react'

const ReloadCoinDataButton = ({ fetchCoins }) => {

    const handleCoinsReload = () => {
        fetchCoins();
    };

    return (
        <div>
            <button onClick={handleCoinsReload}>Update Portfolio</button>
        </div>
    )
}

export default ReloadCoinDataButton
