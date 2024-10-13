import React from 'react'
import CoinTable from './CoinTable'
import Header from './Header'

const CoinPortfolio = ({ coins }) => {
  return (
    <>
      <Header />
      <CoinTable coins={coins} />
    </>
  )
}

export default CoinPortfolio
