import React from 'react'
import CoinTable from './CoinTable'
import CoinTableHeader from './CoinTableHeader'

const CoinPortfolio = ({ coins }) => {
  return (
    <>
      <CoinTableHeader />
      <CoinTable coins={coins} tableCaption={'Your Owned Coins'}/>
    </>
  )
}

export default CoinPortfolio
