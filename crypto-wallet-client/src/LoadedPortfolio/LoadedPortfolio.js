import React from 'react'
import CoinPortfolio from './CoinPortfolio/CoinPortfolio'
import DeletePortfolio from './DeletePortfolio'
import InitialPortfolioValue from './InitialPortfolioValue'
import CurrentPortfolioValue from './CurrentPortfolioValue'

const LoadedPortfolio = ({ coins, setFileUploaded, fetchCoins, checkIfHasCoins }) => {
    return (
        <div className='loadedPortfolio'>
            <CoinPortfolio coins={coins} fetchCoins={fetchCoins} />
            <DeletePortfolio setFileUploaded={setFileUploaded} checkIfHasCoins={checkIfHasCoins} />
            <InitialPortfolioValue />
            <CurrentPortfolioValue />
        </div>
    )
}

export default LoadedPortfolio