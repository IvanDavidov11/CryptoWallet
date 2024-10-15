import React from 'react'
import CoinTable from './CoinTable'
import CoinTableHeader from './CoinTableHeader'

const CoinPortfolio = ({ coins }) => {
  const hasApiCalculations = true;

  return (
    <>
      <CoinTableHeader />
      <CoinTable coins={coins} tableCaption={'Your Owned Coins'} hasApiCalculations={hasApiCalculations} />
    </>
  )
}

export default CoinPortfolio