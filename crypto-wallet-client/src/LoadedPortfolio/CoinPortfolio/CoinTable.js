import React from 'react'
import CoinRow from './CoinRow'

const CoinTable = ({ coins, tableCaption, hasApiCalculations }) => {
    return (
        <div className='table_component'>

            <table>
                <caption>{tableCaption}</caption>
                <thead>
                    <tr>
                        <th>Coin Name</th>
                        <th>Amount</th>
                        <th>Bought For</th>
                        {hasApiCalculations &&
                            <th>Change from initial price</th>
                        }

                    </tr>
                </thead>
                <tbody>
                    {coins.map((coin) =>
                        <CoinRow key={coin.id} coin={coin} hasApiCalculations={hasApiCalculations}/>
                    )}
                </tbody>
            </table>
        </div>
    )
}

export default CoinTable
