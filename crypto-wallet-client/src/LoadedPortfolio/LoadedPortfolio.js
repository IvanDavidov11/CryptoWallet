import React from 'react'
import CoinPortfolio from './CoinPortfolio/CoinPortfolio'
import ClearPortfolio from './ClearPortfolio'
import InitialPortfolioValue from './InitialPortfolioValue'

const LoadedPortfolio = ({ coins, setFileUploaded}) => {
    return (
        <div className='loadedPortfolio'>
            <ClearPortfolio setFileUploaded={setFileUploaded}/>
            <CoinPortfolio coins={coins} />
            <InitialPortfolioValue />
        </div>
    )
}

export default LoadedPortfolio
