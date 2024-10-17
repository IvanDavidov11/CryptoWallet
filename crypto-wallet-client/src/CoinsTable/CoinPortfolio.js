import React from 'react'
import CoinsTable from './CoinsTable';

const CoinPortfolio = ({ coins }) => {
  const hasApiCalculations = true;

  return (
    <>
      <CoinsTable coins={coins} tableCaption={'Your Positions'} hasApiCalculations={hasApiCalculations} />
    </>
  )
}

export default CoinPortfolio