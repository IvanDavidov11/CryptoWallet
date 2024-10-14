import React from 'react'
import CoinPortfolio from './CoinPortfolio/CoinPortfolio'
import ClearPortfolio from './ClearPortfolio'
import InitialPortfolioValue from './InitialPortfolioValue'
import CurrentPortfolioValue from './CurrentPortfolioValue'

const LoadedPortfolio = ({ coins, setFileUploaded}) => {
    return (
        <div className='loadedPortfolio'>
            <ClearPortfolio setFileUploaded={setFileUploaded}/>
            <CoinPortfolio coins={coins} />
            <InitialPortfolioValue />
            <CurrentPortfolioValue />
        </div>
    )
}

export default LoadedPortfolio
