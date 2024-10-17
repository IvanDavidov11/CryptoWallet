import React from 'react'
import CoinsTable from './CoinsTable';
import PortfolioRefresh from '../LoadedPortfolio/PortfolioRefresh/PortfolioRefresh'

const CoinPortfolio = ({ coins, fetchCoins }) => {
  const hasApiCalculations = true;

  return (
    <>
      <PortfolioRefresh fetchCoins={fetchCoins} />
      <CoinsTable coins={coins} tableCaption={'Your Owned Coins'} hasApiCalculations={hasApiCalculations} />
    </>
  )
}

export default CoinPortfolio